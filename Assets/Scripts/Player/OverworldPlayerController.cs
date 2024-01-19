using UnityEngine;

public class OverworldPlayerController : MonoBehaviour
{
    public float moveSpeed = 0.1f;

    private Vector2 moveInput;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize(); // This ensures consistent movement speed in all directions
    }

    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
