using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStageEnemy : MonoBehaviour
{
    // Patrol Path
    public Transform[] targets;

    // VoiceOrb
    public Transform theOrb;

    public Transform theUser;

    public float PatrolRange;
    private float sqrPatrolRange;

   public enum EnemyState
   {
        IDLE,
        PATROL,
        CHASEORB,
        CHASEPLAYER,
        ATTACK,
        LENGTH
   }

    public EnemyState nowState;

    [Range(0,1)]
    public float idleProb = 0.5f;

    public float updateStatePeriod;    
    public float chasingOrbSpeed = 1.0f;
    public float chasingPlayerSpeed = 1.0f;
    public float patrolSpeed = 1.0f;
    public float patrolSwitchTargetRange = 5.0f;
    private float sqrPatrolSwitchTargetRange = 5.0f;

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

        Vector3 postProcessedTargetPos = targets[currentPatrolTarget].position;
        postProcessedTargetPos.y = startY;

        Quaternion lookOnLook = Quaternion.LookRotation((postProcessedTargetPos - transform.position).normalized);
        transform.rotation = lookOnLook;

        sqrPatrolRange = PatrolRange * PatrolRange;
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
        float orbSqrDistance = GetSqrDistanceWithoutReferenceYAxis(transform.position, theOrb.transform.position);
        float playerSqrDistance = GetSqrDistanceWithoutReferenceYAxis(transform.position, theUser.transform.position);

        print(orbSqrDistance <= sqrPatrolRange);
        print(!orbContainer.playerOrbs[0]);

        // if isNear && Player throw orb
        if (orbSqrDistance <= sqrPatrolRange && orbContainer.playerOrbs[0] == theOrb.gameObject)
        {
            nowState = EnemyState.CHASEORB;
            innerTime = 0;
        }
        else if (playerSqrDistance <= sqrPatrolRange)
        {
            nowState = EnemyState.CHASEPLAYER;
            innerTime = 0;
        }
        else
        {
            if(nowState == EnemyState.CHASEORB || nowState == EnemyState.CHASEPLAYER)
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
        //int nearsetIndex = FindNearestNode();
        //status = 3;
        // 
    }
}
