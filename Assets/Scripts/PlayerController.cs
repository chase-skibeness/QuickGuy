using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Transform playerTransform;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool facingRight = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack1");
        }

        horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

        animator.SetFloat("MoveSpeed", Math.Abs(horizontalMove));
    }

    private void FixedUpdate()
    {

        playerTransform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime);

        if (horizontalMove > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip() 
    {
        facingRight = !facingRight;

        Vector3 theScale = playerTransform.localScale;
        theScale.x *= -1;
        playerTransform.localScale = theScale;
    }
}
