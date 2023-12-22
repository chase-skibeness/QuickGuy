using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public Rigidbody2D rb;
    

    public float runSpeed = 1f;

    float horizontalMove = 0f;
    bool facingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack1");
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * Time.fixedDeltaTime;

        animator.SetFloat("MoveSpeed", Math.Abs(Input.GetAxisRaw("Horizontal") * runSpeed));
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(horizontalMove * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, .05f);
        rb.AddForce(targetVelocity);

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

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
