using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetInteger("AttackAmount", animator.GetInteger("AttackAmount") + 1);
            if (animator.GetInteger("AttackAmount") > 3 ) { animator.SetInteger("AttackAmount", 0); }
        }
    }
}
