using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOrbContainer : MonoBehaviour
{
    [SerializeField]
    public int playerOrbMaxAmount;

    public GameObject playerGrabOrb;

    public GameObject playerShootedOrb;

    public GameObject[] Orbs;

    void Start()
    {
        //playerOrbs = new GameObject[playerOrbMaxAmount];
    }

    public void Grab(GameObject theOrb)
    {
        playerGrabOrb = theOrb;
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
