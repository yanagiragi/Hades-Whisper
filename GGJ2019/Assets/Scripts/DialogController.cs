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
                this.gameObject.SetActive(false);
                Next.SetActive(true);
            }
            else
            {
                End.SetActive(true);
            }

        }
    }

}
