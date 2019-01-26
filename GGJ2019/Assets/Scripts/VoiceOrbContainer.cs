using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOrbContainer : MonoBehaviour
{
    [SerializeField]
    public int playerOrbMaxAmount;

    public GameObject[] playerOrbs;

    public GameObject[] Orbs;

    public Transform fixedJointTarget;

    void Start()
    {
        playerOrbs = new GameObject[playerOrbMaxAmount];
    }

    public void Grab(int index, GameObject theOrb)
    {
        if (index >= playerOrbMaxAmount)
        {
            Debug.LogError("Attempt to store orb on index: " + index);
        }
        else
        {
            theOrb.transform.position = fixedJointTarget.position;

            playerOrbs[index] = theOrb;

            theOrb.GetComponent<FollowPosition>().target = fixedJointTarget.transform;

            /*FixedJoint fj = Orbs[index].AddComponent<FixedJoint>();
            fj.connectedBody = fixedJointTarget;
            fj.breakForce = Mathf.Infinity;
            fj.breakTorque = Mathf.Infinity;*/
        }   
    }

    public void Grab(GameObject theOrb)
    {
        this.Grab(0, theOrb);
    }

    public int GetNearestOrb(Transform playerTransform)
    {
        float minDistance = Mathf.Infinity;
        int minIndex = -1;

        for(int i = 0; i < Orbs.Length; ++i)
        {
            float distance = (Orbs[i].transform.position - playerTransform.position).sqrMagnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }

        return minIndex;
    }
}
