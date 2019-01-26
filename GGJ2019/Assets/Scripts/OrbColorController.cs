using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbColorController : MonoBehaviour
{
    void OnEnable()
    {
        Renderer rend = GetComponent<Renderer>();
        Light light = GetComponentInChildren<Light>();

        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", light.color);
    }
}
