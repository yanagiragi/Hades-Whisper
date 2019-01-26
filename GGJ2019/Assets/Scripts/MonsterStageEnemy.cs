using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStageEnemy : MonoBehaviour
{
    // Patrol Path
    public GameObject[] targets;

    // VoiceOrb
    public GameObject theOrb;
    public GameObject theUser;
    public float PatrolRange;
    public float startTime;

    // status
    // 1: chase
    // 2: idle
    public int status;
    public int i;
    public int targets_num;
    public bool passive;

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

    [SerializeField]
    private float innerTime;

    public float chasingOrbSpeed;
    public float chasingPlayerSpeed;
    public float patrolSpeed;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        this.Initialize();
    }

    private void Initialize()
    {
        i = 0;
        status = 1;
        if(!anim)
            anim = GetComponent<Animator>();
    }

    public void UpdateState()
    {
        float orbSqrDistance = (transform.position - theOrb.transform.position).sqrMagnitude;
        float playerSqrDistance = (transform.position - theUser.transform.position).sqrMagnitude;

        if (orbSqrDistance < PatrolRange)
        {
            nowState = EnemyState.CHASEORB;
        }
        else if (playerSqrDistance < PatrolRange)
        {
            nowState = EnemyState.CHASEPLAYER;
        }
        else
        {
            float rand = Random.Range(0, 1);
            if (rand > idleProb)
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

    public void Patrol()
    {
        // Go to patrol node
        float distance = (transform.position - targets[i].transform.position).sqrMagnitude;

        if (distance < 5.0f && status == 1)
        {
            i += 1;
            i = i % targets.Length;
            startTime = Time.time;
            status = 2;
            anim.SetBool("isWalk", false);
        }
        else if (status == 2)
        {
            Quaternion lookOnLook = Quaternion.LookRotation((targets[i].transform.position - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);
            // idle
            if ((Time.time - startTime) > 3)
            {
                status = 1;
            }
        }
        else
        {
            anim.SetBool("isWalk", true);
            Chase(transform.position, targets[i].transform.position, patrolSpeed);
        }
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

    // Update is called once per frame
    void Update()
    {
        innerTime += Time.deltaTime;

        if(innerTime > updateStatePeriod)
        {
            innerTime = 0;
            UpdateState();
            DealState();
        }
    }

    void Chase(Vector3 startPos, Vector3 targetPos, float speedFactor)
    {
        transform.position = Vector3.Lerp(startPos, targetPos, Time.deltaTime * speedFactor);
        Quaternion lookOnLook = Quaternion.LookRotation((targetPos - startPos).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);
    }

    void FindNearestNode()
    {
        int j;
        int index = 0;
        float min_distance = Mathf.Infinity;
        for (j = 0; j < targets_num; j+=1)
        {
            if(min_distance > (targets[j].transform.position - transform.position).sqrMagnitude)
            {
                min_distance = (targets[j].transform.position - transform.position).sqrMagnitude;
                index = j;
            }
        }

        i = index;
    }

    void OnTriggerEnter(Collider other)
    {
        FindNearestNode();
        status = 3;
        // 
    }
}
