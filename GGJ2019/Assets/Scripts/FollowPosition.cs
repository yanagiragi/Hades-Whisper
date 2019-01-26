using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f;
    
    void Start()
    {
        
    }

    void Update()
    {
        if(target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
        }
    }
}
