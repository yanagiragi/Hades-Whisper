using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStageEnemy : MonoBehaviour
{
    public bool willAtkPlayer = true;

    // Patrol Path
    public Transform[] targets;

    // VoiceOrb
    public Transform theOrb;

    public Transform theUser;

    public float PatrolRange;
    private float sqrPatrolRange;

    public float prefromingEatTimeLimit = 3.0f;

    public Transform mouthTransfrom;

    public GameObject FadeOutOrb;

    public enum EnemyState
   {
        IDLE,
        PATROL,
        CHASEORB,
        CHASEPLAYER,
        ATTACK,
        EATTING,
        LOOPIDLE,
        LENGTH
   }

    public EnemyState nowState;

    [Range(0,2)]
    public float idleProb = 0.5f;

    public float updateStatePeriod;    
    public float chasingOrbSpeed = 1.0f;
    public float chasingPlayerSpeed = 1.0f;
    public float patrolSpeed = 1.0f;
    public float patrolSwitchTargetRange = 5.0f;
    private float sqrPatrolSwitchTargetRange = 5.0f;
    public float startBiteDistance;
    private float sqrStartBiteDistance;

    public float rotateSpeedFactor = 1.0f;

    public VoiceOrbContainer orbContainer;

    [Header("SerialzedField")]

    [SerializeField]
    private int currentPatrolTarget = 0;

    [SerializeField]
    private float innerTime;

    private float startY;

    private Animator anim;

    void Start()
    {
        this.Initialize();

        startY = transform.position.y;

        // Only On Start
        UpdateState();

        if (targets.Length > 0)
        {
            Vector3 postProcessedTargetPos = targets[currentPatrolTarget].position;
            postProcessedTargetPos.y = startY;

            Quaternion lookOnLook = Quaternion.LookRotation((postProcessedTargetPos - transform.position).normalized);
            transform.rotation = lookOnLook;
        }
      

        sqrPatrolRange = PatrolRange * PatrolRange;

        sqrStartBiteDistance = startBiteDistance * startBiteDistance;
    }

    private void Initialize()
    {
        sqrPatrolSwitchTargetRange = patrolSwitchTargetRange * patrolSwitchTargetRange;
        if (!anim)
            anim = GetComponent<Animator>();
    }

    public void UpdateState()
    {
        if(nowState != EnemyState.CHASEPLAYER && nowState != EnemyState.CHASEORB)
        {
            float rand = Random.Range(0.0f, 1.0f);
            if (rand >= idleProb)
            {
                nowState = EnemyState.PATROL;
            }
            else
            {
                nowState = EnemyState.IDLE;
            }
        }
    }

    public void ChaseOrb()
    {
        Chase(transform.position, theOrb.transform.position, chasingOrbSpeed);
    }

    public void ChasePlayer()
    {
        Chase(transform.position, theUser.transform.position, chasingPlayerSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, PatrolRange);

        for(int i = 0; i < targets.Length; ++i)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(targets[i].position, patrolSwitchTargetRange);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, startBiteDistance);
    }

    public void Patrol()
    {
        // Go to patrol node
        float sqrDistance = GetSqrDistanceWithoutReferenceYAxis(transform.position, targets[currentPatrolTarget].position);

        if (sqrDistance <= sqrPatrolSwitchTargetRange)
        {
            // switch target
            currentPatrolTarget = (currentPatrolTarget + 1) % targets.Length;
        }

        Vector3 postProcessedTargetPos = targets[currentPatrolTarget].position;
        postProcessedTargetPos.y = startY;

        Quaternion lookOnLook = Quaternion.LookRotation((postProcessedTargetPos - transform.position).normalized);
        transform.rotation = lookOnLook;

        anim.SetBool("isWalk", true);
        Chase(transform.position, targets[currentPatrolTarget].transform.position, patrolSpeed);

    }

    public void Idle()
    {
        anim.SetBool("isAttack", false);
        anim.SetBool("isWalk", false);
    }

    public void DealState()
    {
        switch (nowState)
        {
            case EnemyState.IDLE:
                Idle();
                break;
            case EnemyState.CHASEORB:
                ChaseOrb();
                break;
            case EnemyState.CHASEPLAYER:
                ChasePlayer();
                break;
            case EnemyState.ATTACK:
                // not yet
                break;
            case EnemyState.PATROL:
                Patrol();
                break;
        }
    }

    float GetSqrDistanceWithoutReferenceYAxis(Vector3 pos1, Vector3 pos2)
    {
        Vector3 dis = pos1 - pos2;
        dis.y = 0;
        return dis.sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if(nowState != EnemyState.EATTING)
        {
            if(!theOrb && nowState == EnemyState.CHASEPLAYER && willAtkPlayer)
            {
                // Already Eated, End Phase
                anim.SetBool("isRun", true);
                DealState();
                return;
            }
            else if((nowState == EnemyState.LOOPIDLE && !willAtkPlayer))
            {
                // Already Eated, End Phase
                anim.SetBool("isAttack", true);
                DealState();
                return;
            }

            float orbSqrDistance = GetSqrDistanceWithoutReferenceYAxis(transform.position, theOrb.transform.position);
            float playerSqrDistance = GetSqrDistanceWithoutReferenceYAxis(transform.position, theUser.transform.position);

            // print(string.Format("{0} {1}", orbSqrDistance <= sqrPatrolRange, orbContainer.playerShootedOrb == theOrb));

            // if isNear && Player throw orb
            // Player throw orb need to be checked
            if (orbSqrDistance <= sqrPatrolRange && orbContainer.playerShootedOrb && theOrb.GetComponent<Renderer>().enabled)
            {
                nowState = EnemyState.CHASEORB;
                innerTime = 0;
            }
            else if (willAtkPlayer && playerSqrDistance <= sqrPatrolRange)
            {
                nowState = EnemyState.CHASEPLAYER;
                innerTime = 0;
            }
            else
            {
                if (nowState == EnemyState.CHASEORB || nowState == EnemyState.CHASEPLAYER)
                {
                    nowState = EnemyState.IDLE;
                }

                innerTime += Time.deltaTime;

                if (innerTime > updateStatePeriod)
                {
                    innerTime = 0;
                    UpdateState();
                }
            }

            DealState();
        }
    }

    void Chase(Vector3 startPos, Vector3 targetPos, float speedFactor)
    {
        transform.position = Vector3.Lerp(startPos, targetPos, Time.deltaTime * speedFactor);

        // post process on position.y
        Vector3 pos = transform.position;
        pos.y = startY;
        transform.position = pos;

        Vector3 postProcessedTargetPos = targetPos;
        postProcessedTargetPos.y = startY;

        Quaternion lookOnLook = Quaternion.LookRotation((postProcessedTargetPos - startPos).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * rotateSpeedFactor);

        anim.SetBool("isWalk", true);
    }

    int FindNearestTarget()
    {
        int minIndex = 0;
        float min_distance = Mathf.Infinity;

        for (int j = 0; j < targets.Length; j+=1){
            float d = (targets[j].transform.position - transform.position).sqrMagnitude;
            if (min_distance > d) {
                min_distance = d;
                minIndex = j;
            }
        }

        return minIndex;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Collective"))
        {
            if (nowState != EnemyState.EATTING)
            {
                nowState = EnemyState.EATTING;
                StartCoroutine(PerformEating());
            }
        }
        else if (willAtkPlayer && other.gameObject.CompareTag("Player"))
        {
            TriggerAttackPlayer();
        }
    }

    public void EatBall(GameObject theOrb)
    {
        FadeOutOrb = theOrb;
        theOrb.GetComponent<Rigidbody>().useGravity = false;
        theOrb.GetComponent<Rigidbody>().isKinematic = false;
        theOrb.GetComponent<Collider>().enabled = false;

        FadeOutOrb.GetComponent<FollowPosition>().target = mouthTransfrom;
        FadeOutOrb.GetComponent<FollowPosition>().speed = 100f;
    }

    public void Stop()
    {
        anim.speed = 0.0f;
        nowState = EnemyState.EATTING;
        this.enabled = false;
    }

    IEnumerator PerformEating()
    {
        float time = 0.0f;

        while(time <= prefromingEatTimeLimit)
        {
            float sqrdistance = GetSqrDistanceWithoutReferenceYAxis(theOrb.position, transform.position);

            // print(sqrdistance);

            if (sqrdistance <= sqrStartBiteDistance)
            {
                anim.SetTrigger("isBiting");

                anim.SetBool("isWalk", false);

                anim.SetBool("isAttack", true);

                // Remove
                orbContainer.Grab(theOrb.gameObject);

                EatBall(theOrb.gameObject);

                break;
            }
            else
            {
                ChaseOrb();
            }

            time += Time.deltaTime;

            if(FadeOutOrb)
            {
                Material mat = FadeOutOrb.GetComponent<Renderer>().material;
                Color col = mat.GetColor("_EmissionColor");
                // col = Color.Lerp(col, Color.black, Time.deltaTime);
                //mat.SetColor("_EmissionColor", col);
                FadeOutOrb.GetComponent<AudioSource>().volume -= Time.deltaTime;
            }

            yield return null;
        }

        while (time <= prefromingEatTimeLimit)
        {
            time += Time.deltaTime;

            if (FadeOutOrb)
            {
                Material mat = FadeOutOrb.GetComponent<Renderer>().material;
                Color col = mat.GetColor("_EmissionColor");
                // col = Color.Lerp(col, Color.black, Time.deltaTime);
                //mat.SetColor("_EmissionColor", col);
                FadeOutOrb.GetComponent<AudioSource>().volume -= Time.deltaTime;
            }

            yield return null;
        }

        // anim.SetBool("isBiting", false);
        if (willAtkPlayer)
        {
            Destroy(FadeOutOrb);
            FadeOutOrb = null;
        }

        if (willAtkPlayer)
            nowState = EnemyState.CHASEPLAYER;
        else
            nowState = EnemyState.LOOPIDLE;
    }

    public void TriggerAttackPlayer()
    {
        // Time.timeScale = 0.0f;

    

        chasingPlayerSpeed = 0.0f;

        GlobalLevelManager.instance.Player.GetComponent<DeathCam>().isDead = true;

        GlobalLevelManager.instance.Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false;

        GlobalLevelManager.instance.Dead();
    }
}
