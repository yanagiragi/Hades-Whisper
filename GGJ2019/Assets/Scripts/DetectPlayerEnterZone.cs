using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerEnterZone : MonoBehaviour
{
    public int index = -1;

    public GameObject wallCollider;

    public GlobalLevelManager globalLevelManager;

    public void Start()
    {
        if(index == -1)
            Debug.LogError("Warning on unmodified index!");

        wallCollider.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            wallCollider.SetActive(true);
            this.enabled = false;
            globalLevelManager.InvokeEnterZone(index);
        }
    }
}
