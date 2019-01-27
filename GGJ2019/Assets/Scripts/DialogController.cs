using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public GameObject Next;
    public GameObject End;

    [Header("Ending Only")]
    public float factor;
    public AudioSource endingBGM;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if(Next != null){
                Next.SetActive(true);
                StartCoroutine(Turnoffball());
            }
            else
            {
                End.SetActive(true);
                StartCoroutine(Endings());
                this.enabled = false;
            }

        }
    }

    IEnumerator Turnoffball()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);

    }

    IEnumerator Endings()
    {
        yield return new WaitForSeconds(8f);
        
        while(endingBGM.volume > 0)
        {
            endingBGM.volume -= Time.deltaTime * factor;
            yield return new WaitForSeconds(0.1f);
        }
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("NewScene");
    }

}
