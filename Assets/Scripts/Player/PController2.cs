using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PController2 : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    [SerializeField]
    private float moveSpeed = 100f;

    [SerializeField]
    private float jumpForce = 7.5f;

    [SerializeField]
    private int maxCombo = 3;

    [SerializeField]
    private float attackCooldownTime = 2f;

    [SerializeField]
    private int maxHealth = 5;

    private int health;
    private float attackCooldownTimer = 0f;
    private bool attackCooldown = false;
    private int comboStep = 0;
    private float timeSinceLastAttack = 0f;
    private const float comboTimingWindow = 0.75f;
    private float horizontalInput;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool facingRight = true;
    private bool isBlocking = false;
    private bool isDisabled = false;
    private float timeToEnabled;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    void Update()
    {

        UpdateHealthBar();

        if (health <= 0)
        {
            isDisabled = true;
            animator.SetTrigger("Death");
            Destroy(gameObject, 2f);
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

        if (Mathf.Abs(horizontalInput) > 0 && isGrounded && !isBlocking)
        {
            animator.SetBool("IsWalking", true);
        } else
        {
            animator.SetBool("IsWalking", false);
        }

        // Jump logic
        if ((isGrounded || !isGrounded && canDoubleJump) && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) && !isDisabled && !isBlocking)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset the y velocity
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);

            if (!isGrounded)
            {
                canDoubleJump = false; // Double jump used
            }
        }

        // attack logic
        if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) && !attackCooldown && !isDisabled && !isBlocking)
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

        if ((Input.GetMouseButton(1) || Input.GetButton("Fire2")) && !isDisabled)
        {
            animator.SetBool("Blocking", true);
            isBlocking = true;
        } else if ((Input.GetMouseButtonUp(1) || Input.GetButtonUp("Fire2")))
        {
            animator.SetBool("Blocking", false);
            isBlocking = false;
        }


    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isGrounded && !isDisabled && !isBlocking)
        {
            currentSpeed *= 2.0f; // Double the speed if Shift is held
        }

        float horizontalMovement = horizontalInput * currentSpeed * Time.deltaTime;

        if (!isDisabled && !isBlocking)
        {
            rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);
        }

        if (horizontalMovement > 0 && !facingRight && !isDisabled && !isBlocking)
        {
            Flip();
        }
        else if (horizontalMovement < 0 && facingRight && !isDisabled && !isBlocking)
        {
            Flip();
        }
    }

    void UpdateHealthBar()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            } else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            } else
            {
                hearts[i].enabled = false;
            }
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
        if (isBlocking)
        {
            animator.SetTrigger("Blocked");
        } else
        {
            animator.SetTrigger("Damaged");
            health -= damage;
        }
    }

    public void Disable(float disableLength)
    {
        isDisabled = true;
        timeToEnabled = disableLength;
    }

}
