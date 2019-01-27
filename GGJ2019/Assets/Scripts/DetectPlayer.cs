using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public Transform playerTransform;

    void Start()
    {
        this.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.GetComponent<Renderer>().enabled = true;
            this.enabled = false;
        }
    }
}
