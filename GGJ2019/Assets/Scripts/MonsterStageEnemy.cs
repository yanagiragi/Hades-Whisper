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

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        status = 1;
        anim = GetComponent<Animator>();
    }

    public void ChaseOrb()
    {
        // Find the orb
        status = 1;
        chase(transform.position, theOrb.transform.position);
        Debug.Log("Find Orb");
    }

    public void ChasePlayer()
    {
        // Find the user
        status = 1;
        chase(transform.position, theUser.transform.position);
        Debug.Log("Find User");
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - theOrb.transform.position).sqrMagnitude < PatrolRange)
        {
            // Find the orb
            ChaseOrb();
        } else if ((transform.position - theUser.transform.position).sqrMagnitude < PatrolRange)
        {
            // Find the user
            ChasePlayer();
        }
        else if(passive == false){
            // Go to patrol node
            float distance = (transform.position - targets[i].transform.position).sqrMagnitude;

            if ( distance < 5.0f && status==1)
            {
                i += 1;
                i = i % targets.Length;
                startTime = Time.time;
                status = 2;
                anim.SetBool("isWalk", false);
            } else if(status==2)
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
                chase(transform.position, targets[i].transform.position);
            }
        }
    }

    void chase(Vector3 startPos, Vector3 targetPos)
    {
            transform.position = Vector3.Lerp(startPos, targetPos, Time.deltaTime * 0.5f);
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
