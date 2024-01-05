using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    private Transform playerTransform;
    Animator animator;

    [SerializeField]
    public float speed = 2.5f;

    [SerializeField]
    public float detectionRange = 5.0f;

    [SerializeField]
    private int health = 3;

    private float givenSpace = 2f;

    public float attackTimer = 0f;
    public float attackWaitTime = 2f;
    public float attackRange = 3f;

    private bool isDisabled = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        attackTimer += Time.deltaTime;

        if ((distanceToPlayer <= attackRange) && attackTimer >= attackWaitTime && !isDisabled)
        {
            animator.SetTrigger("Attack");
            attackTimer = 0f;
        }
        {
             
        }

        if ((distanceToPlayer < detectionRange) && (distanceToPlayer > givenSpace) && !isDisabled)
        {
            if (Math.Abs(rb.velocity.x) < speed)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                direction.y = 0;
                rb.AddForce(direction * speed);
            }
            
        }
    }

    public void ApplyDamage(int damage)
    {
        animator.SetTrigger("Damaged");
        health -= damage;
    }

    private void Death()
    {
        isDisabled = true;
        animator.SetTrigger("Death");
        Destroy(this.gameObject, 2f);
    }
}
