using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerEnterZone : MonoBehaviour
{
    public int index = -1;

    public GameObject wallCollider;

    public Transform OrbDropPoint;

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

            // if Player already hold an orb, drop it to prevent he carrys to next level
            VoiceOrbContainer orbContainer = other.gameObject.GetComponent<PlayerController>().orbContainer;

            GameObject playerGrabOrb = orbContainer.playerGrabOrb;

            if (playerGrabOrb)
            {
                DropOrb(orbContainer, playerGrabOrb);
            }

            orbContainer.playerShootedOrb = null;

            GlobalLevelManager.instance.InvokeEnterZone(index, this);

            this.enabled = false;
        }
    }

    public void DropOrb(VoiceOrbContainer orbContainer, GameObject playerGrabOrb)
    {
        playerGrabOrb.GetComponent<AudioSource>().volume = 0.0f;

        if ((playerGrabOrb.transform.position - playerGrabOrb.GetComponent<Orb>().startPosition).sqrMagnitude < 0.1f)
        {
            // almost same place, no need to force tranlate to last stage's orb drop point
            return;
        }

        orbContainer.playerShootedOrb = orbContainer.playerGrabOrb;
        orbContainer.playerGrabOrb = null;

        playerGrabOrb.GetComponent<FollowPosition>().enabled = false;

        playerGrabOrb.transform.position = OrbDropPoint.position;

        Rigidbody rb = playerGrabOrb.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
