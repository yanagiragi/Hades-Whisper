using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLevelManager : MonoBehaviour
{
    public Light[] levelOneLights;
    public Light[] levelTwoLights;
    public Light[] levelThreeLights;

    public GameObject[] levelOneEmmisiveGameObjects;
    public GameObject[] levelTwoEmmisiveGameObjects;
    public GameObject[] levelThreeEmmisiveGameObjects;

    public Material placeHolderMaterial;

    public GameObject Player;

    public Transform RespawnPoint;

    public Transform CheckPoint;

    void Start()
    {
        Player.transform.position = RespawnPoint.position;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeEnterZone(int index)
    {
        // ${index} zone means we leave zone ${index} and need to turn off zone ${index}'s lights
        switch (index)
        {
            case 1:
                for(int i = 0; i < levelOneLights.Length; ++i)
                {
                    levelOneLights[i].enabled = false;
                }
                for (int i = 0; i < levelOneEmmisiveGameObjects.Length; ++i)
                {
                    levelOneEmmisiveGameObjects[i].GetComponent<Renderer>().material = placeHolderMaterial;
                }
                    break;
            case 2:
                for (int i = 0; i < levelTwoLights.Length; ++i)
                {
                    levelTwoLights[i].enabled = false;
                }
                break;
            case 3:
                for (int i = 0; i < levelThreeLights.Length; ++i)
                {
                    levelThreeLights[i].enabled = false;
                }
                break;
            default:
                Debug.LogError("Error Index of Enter Zone: " + index);
                break;
        }
    }
}
