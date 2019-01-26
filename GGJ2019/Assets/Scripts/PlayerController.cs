using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isCarrying = false;

    public float fetchDistance;

    private float squaredFetchDistance;

    public VoiceOrbContainer orbContainer;

    void Start()
    {
        isCarrying = false;
        if (!orbContainer)
        {
            orbContainer = GetComponent<VoiceOrbContainer>();
        }

        squaredFetchDistance = fetchDistance * fetchDistance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isCarrying)
            {
                Fire();
            }
            else
            {
                
                GameObject nearestOrb = orbContainer.Orbs[orbContainer.GetNearestOrb(transform)];

                if (nearestOrb)
                {
                    float distance = (nearestOrb.transform.position - transform.position).sqrMagnitude;

                    if (distance < squaredFetchDistance)
                        Collect(nearestOrb);
                    else if(Application.isEditor)
                    {
                        Debug.LogWarning("Not in Fetch Distance");
                    }
                }                
            }
        }
    }

    void Fire()
    {
        // Fire the Voice Orb
    }

    void Collect(GameObject nearestOrb)
    {
        // Collect the Voice Orb
        orbContainer.Grab(nearestOrb);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fetchDistance);
    }
}
