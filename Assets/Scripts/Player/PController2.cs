using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PController2 : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [SerializeField]
    private float moveSpeed = 100f;

    [SerializeField]
    private float jumpForce = 7.5f;

    [SerializeField]
    private int maxCombo = 3;

    [SerializeField]
    private float attackCooldownTime = 2f;

    [SerializeField]
    private int health = 10;

    private float attackCooldownTimer = 0f;
    private bool attackCooldown = false;
    private int comboStep = 0;
    private float timeSinceLastAttack = 0f;
    private const float comboTimingWindow = 0.75f;
    private float horizontalInput;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (health <= 0)
        {
            animator.SetTrigger("Death");
        }

        timeSinceLastAttack += Time.deltaTime;

        if (attackCooldown)
        {
            attackCooldownTimer += Time.deltaTime;
            if (attackCooldownTimer >= attackCooldownTime)
            {
                attackCooldown = false;
                comboStep = 0;
                attackCooldownTimer = 0f;
                ResetTriggers();
            }
        }

        horizontalInput = Input.GetAxis("Horizontal");

        // Jump logic
        if ((isGrounded || !isGrounded && canDoubleJump) && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset the y velocity
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);

            if (!isGrounded)
            {
                canDoubleJump = false; // Double jump used
            }
        }

        // attack logic
        if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) && !attackCooldown)
        {
            if ((timeSinceLastAttack <= comboTimingWindow) && comboStep < maxCombo)
            {
                comboStep++;
                ExecuteComboAttack(comboStep);

                if (comboStep == maxCombo)
                {
                    attackCooldown = true;
                    attackCooldownTimer = 0f;
                }
            } else
            {
                ResetTriggers();
                comboStep = 1;
                ExecuteComboAttack(comboStep);
            }
            timeSinceLastAttack = 0;
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isGrounded)
        {
            currentSpeed *= 2.0f; // Double the speed if Shift is held
        }

        float horizontalMovement = horizontalInput * currentSpeed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);

        if (horizontalMovement > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalMovement < 0 && facingRight)
        {
            Flip();
        }
    }

    // Detect collision with the ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = true; // Reset double jump when the player touches the ground
        }
    }

    // Detect when leaving the ground
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void ExecuteComboAttack(int step)
    {
        animator.SetTrigger("Attack" + step);
    }

    private void ResetTriggers()
    {
        for (int i = 1; i <= maxCombo; i++)
        {
            animator.ResetTrigger("Attack" + i);
        }
    }

    public void ApplyDamage(int damage)
    {
        animator.SetTrigger("Damaged");
        health -= damage;
    }

    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
