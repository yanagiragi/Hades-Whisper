using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCam : MonoBehaviour
{
    public Transform target;

    public float start = -11.46f;

    public float end = -12.37f;

    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        target.GetComponent<Renderer>().enabled = isDead;
        if (isDead)
        {
            target.position = Vector3.Lerp(target.position, target.position + new Vector3(0, 0, end - start), Time.deltaTime);
        }
    }
}
