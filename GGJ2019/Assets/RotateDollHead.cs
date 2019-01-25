using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDollHead : MonoBehaviour
{
    public float speedX, speedY, speedZ;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.EulerRotation(new Vector3(speedX, speedY, speedZ) * Time.time);
    }
}
