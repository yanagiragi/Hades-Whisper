using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSpiderController : MonoBehaviour
{
    
    public enum EnemyaniType
    {
        attack,
        jump,
        up,

    }
    public EnemyaniType animationType;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animationType.ToString());
        animator.Play(animationType.ToString());
    }

}

