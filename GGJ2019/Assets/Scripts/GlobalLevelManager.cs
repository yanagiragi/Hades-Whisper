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

    public Animator[] levelOneAnimators;
    public Animator[] levelTwoAnimators;
    public Animator[] levelThreeAnimators;

    public Animation[] levelOneAnimations;
    public Animation[] levelTwoAnimations;
    public Animation[] levelThreeAnimations;

    public Material placeHolderMaterial;

    public GameObject Player;

    public Transform RespawnPoint;

    public Transform CheckPoint;

    public int nowLevel = 1;

    public static GlobalLevelManager instance = null;

    public VoiceOrbContainer orbController;

    public Animator gameOverUIAnimator;

    public MonsterStageEnemy DollBehaviour;

    public MonsterStageEnemy SpiderBehaviour;

    public bool isDead;

    private void Awake()
    {
        //if we don't have an [_instance] set yet
        if (!instance)
            instance = this;
        //otherwise, if we do, kill this thing
        else
        {
            instance.RespawnPoint = this.CheckPoint;
            instance.CheckPoint = this.CheckPoint;
            
            instance.levelOneAnimations = this.levelOneAnimations;
            instance.levelOneAnimators = this.levelOneAnimators;
            instance.levelOneEmmisiveGameObjects = this.levelOneEmmisiveGameObjects;

            instance.levelTwoAnimations = this.levelTwoAnimations;
            instance.levelTwoAnimators = this.levelTwoAnimators;
            instance.levelTwoEmmisiveGameObjects = this.levelTwoEmmisiveGameObjects;
            
            instance.Player = this.Player;
            instance.nowLevel = this.nowLevel;
            instance.orbController =this.orbController;
            instance.DollBehaviour = this.DollBehaviour;
            instance.SpiderBehaviour = this.SpiderBehaviour;
            instance.gameOverUIAnimator = this.gameOverUIAnimator;

            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        instance.nowLevel = 1;
        Player.transform.position = isDead ? CheckPoint.position : RespawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeEnterZone(int index, DetectPlayerEnterZone detectPlayerEnterZone)
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

        switch(nowLevel)
        {
            case 1:
                for (int i = 0; i < orbController.Level1Orbs.Count; ++i)
                {
                    if (orbController.Level1Orbs[i])
                        detectPlayerEnterZone.DropOrb(orbController, orbController.Level1Orbs[i]);
                }
                break;
            case 2:
                for (int i = 0; i < orbController.Level2Orbs.Count; ++i)
                {
                    if (orbController.Level2Orbs[i])
                        detectPlayerEnterZone.DropOrb(orbController, orbController.Level2Orbs[i]);
                }
                break;
            case 3:
                for (int i = 0; i < orbController.Level3Orbs.Count; ++i)
                {
                    if (orbController.Level3Orbs[i])
                        detectPlayerEnterZone.DropOrb(orbController, orbController.Level3Orbs[i]);
                }
                break;
            default:
                print("Error on GLM");
                break;
        }

        switch (nowLevel)
        {
            case 1:
                for (int i = 0; i < levelOneAnimators.Length; ++i)
                {
                    levelOneAnimators[i].speed = 0.0f;
                }
                break;
            case 2:
                for (int i = 0; i < levelTwoAnimators.Length; ++i)
                {
                    levelTwoAnimators[i].speed = 0.0f;
                }
                break;
            case 3:
                for (int i = 0; i < levelThreeAnimators.Length; ++i)
                {
                    levelThreeAnimators[i].speed = 0.0f;
                }
                break;
            default:
                print("Error on GLM");
                break;
        }

        switch (nowLevel)
        {
            case 1:
                for (int i = 0; i < levelOneAnimations.Length; ++i)
                {
                    levelOneAnimations[i].enabled = false;
                }
                break;
            case 2:
                for (int i = 0; i < levelTwoAnimations.Length; ++i)
                {
                    levelTwoAnimations[i].enabled = false;
                }
                break;
            case 3:
                for (int i = 0; i < levelThreeAnimations.Length; ++i)
                {
                    levelThreeAnimations[i].enabled = false;
                }
                break;
            default:
                print("Error on GLM");
                break;
        }

        if (nowLevel == 1)
        {
            SpiderBehaviour.Stop();
        }
        else if(nowLevel == 2)
        {
            DollBehaviour.Stop();
        }

        nowLevel = detectPlayerEnterZone.index + 1;

    }

    public void Dead()
    {
        gameOverUIAnimator.SetTrigger("GameOver");

        isDead = true;

        StartCoroutine(Replay());
    }

    IEnumerator Replay()
    {
        yield return new WaitForSecondsRealtime(10f);

        instance.RespawnPoint.position = instance.CheckPoint.position;
        
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainScene");

    }
}
