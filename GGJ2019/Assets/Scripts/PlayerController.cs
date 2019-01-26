using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isCarrying = false;

    public float fetchDistance;

    public float forceFactor = 1.0f;

    public float forwardFactor = 1.0f;

    private float squaredFetchDistance;

    public VoiceOrbContainer orbContainer;

    public Transform fixedJointTarget;

    public Transform mainCameraTransform;

    private GameObject grabbingOrb;
    
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

        grabbingOrb.GetComponent<FollowPosition>().target = null;

        Rigidbody rb = grabbingOrb.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce((mainCameraTransform.forward * forwardFactor + mainCameraTransform.up).normalized * forceFactor, ForceMode.Acceleration);

        isCarrying = false;

        grabbingOrb = null;
    }

    void Collect(GameObject nearestOrb)
    {
        nearestOrb.transform.position = fixedJointTarget.position;

        // Collect the Voice Orb
        orbContainer.Grab(nearestOrb);

        nearestOrb.GetComponent<FollowPosition>().target = fixedJointTarget.transform;

        grabbingOrb = nearestOrb;

        Rigidbody rb = grabbingOrb.GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        isCarrying = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fetchDistance);

        if (grabbingOrb)
            Gizmos.DrawRay(new Ray(fixedJointTarget.position, fixedJointTarget.position + mainCameraTransform.forward * forwardFactor + mainCameraTransform.up));
    }
}
