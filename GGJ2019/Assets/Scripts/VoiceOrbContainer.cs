using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOrbContainer : MonoBehaviour
{
    public GameObject playerGrabOrb;

    public GameObject playerShootedOrb;

    public List<GameObject> Level1Orbs;

    public List<GameObject> Level2Orbs;

    public List<GameObject> Level3Orbs;

    void Start()
    {
        //playerOrbs = new GameObject[playerOrbMaxAmount];
    }

    public void Grab(GameObject theOrb)
    {
        playerGrabOrb = theOrb;

        switch (GlobalLevelManager.instance.nowLevel)
        {
            case 1:
                Level1Orbs.Remove(theOrb); break;
            case 2:
                Level2Orbs.Remove(theOrb);
                break;
            case 3:
                Level3Orbs.Remove(theOrb);
                break;
            default:
                break;
        }
    }

    public void Restore(GameObject theOrb)
    {
        playerGrabOrb = theOrb;

        switch (GlobalLevelManager.instance.nowLevel)
        {
            case 1:
                Level1Orbs.Add(theOrb); break;
            case 2:
                Level2Orbs.Add(theOrb);
                break;
            case 3:
                Level3Orbs.Add(theOrb);
                break;
            default:
                break;
        }
    }

    public GameObject GetNearestOrb(int levelIndex, Transform playerTransform)
    {
        switch (levelIndex)
        {
            case 1:
                return Level1Orbs[GetNearestOrbIndex(Level1Orbs, playerTransform)];
                break;
            case 2:
                return Level2Orbs[GetNearestOrbIndex(Level2Orbs, playerTransform)];
                break;
            case 3:
                return Level3Orbs[GetNearestOrbIndex(Level3Orbs, playerTransform)];
                break;
            default:
                return null;
                break;
        }
    }

    public int GetNearestOrbIndex(int levelIndex, Transform playerTransform)
    {
        switch (levelIndex)
        {
            case 1:
                return GetNearestOrbIndex(Level1Orbs, playerTransform);
                break;
            case 2:
                return GetNearestOrbIndex(Level2Orbs, playerTransform);
                break;
            case 3:
                return GetNearestOrbIndex(Level3Orbs, playerTransform);
                break;
            default:
                return -1;
                break;
        }
    }

    public int GetNearestOrbIndex(List<GameObject> Orbs, Transform playerTransform)
    {
        float minDistance = Mathf.Infinity;
        int minIndex = -1;

        for(int i = 0; i< Orbs.Count; ++i)
        {
            float distance = (Orbs[i].transform.position - playerTransform.position).sqrMagnitude;

            if(distance<minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }

        return minIndex;
    }
}
