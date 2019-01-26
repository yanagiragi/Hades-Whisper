using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameOverFade : MonoBehaviour
{
    private Material mat;

    public float cutoff;

    public void Start()
    {
        mat = GetComponent<UnityEngine.UI.Image>().material;

        ReStore();
    }

    public void Update()
    {
        mat.SetFloat("_cutoff", cutoff);
    }

    public void ReStore()
    {
        mat.SetFloat("_cutoff", 1);
    }
    
}
