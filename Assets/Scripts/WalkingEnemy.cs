using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : enemy
{
    public float moveSpeed = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Move the character
        float horizontalInput = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Flip the character if it reaches the end of the platform
        if ((isFacingRight && !isGrounded) || (!isFacingRight && !isGrounded))
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Flip the character horizontally
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}