using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public GameObject Next;
    public GameObject End;

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
            }

        }
    }

    IEnumerator Turnoffball()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);

    }

}
