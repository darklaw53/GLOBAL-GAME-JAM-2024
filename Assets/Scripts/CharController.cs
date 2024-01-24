using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    public float horizontalInput;
    public bool facingRight = true;

    public int health = 5;
    public GameController gameController;

    public SpriteRenderer spriteRenderer;
    bool iFrames;

    public float blinkDuration = 2f;
    public float blinkInterval = 0.2f;

    bool inputDisabled;
    bool lookingForGround;

    public float knockbackForce = 3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (lookingForGround && isGrounded)
        {
            inputDisabled = false;
        }

        if (!inputDisabled)
        {
            InputDetection();
        }

        if (horizontalInput < 0)
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalInput > 0)
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void InputDetection()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Player shooting
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (!inputDisabled)
        {
            Vector2 moveDirection = new Vector2(horizontalInput, 0f);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "enemy") TakeDamage(1, collision.transform);
    }

    private void Shoot()
    {
        // Instantiate a projectile at the firePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Set the projectile's velocity
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (facingRight) projectileRb.velocity = new Vector2(projectileSpeed, 0f);
        else projectileRb.velocity = new Vector2(-projectileSpeed, 0f);
    }

    public void TakeDamage(int value, Transform source)
    {
        if (!iFrames)
        {
            StartCoroutine(BlinkCharacter());
            if (source.position.x < transform.position.x)
            {
                ApplyKnockback(Vector2.right);
            }
            else
            {
                ApplyKnockback(Vector2.left);
            }

            health -= value;
            if (health < 1) gameController.GameOver();
        }
    }

    IEnumerator LookForGround()
    {
        yield return new WaitForSeconds(1);
        lookingForGround = true;
    }

    IEnumerator BlinkCharacter()
    {
        iFrames = true;
        while (true) // Infinite loop for continuous blinking, you might want to adjust this condition based on your needs
        {
            // Set the character sprite to be invisible
            spriteRenderer.enabled = false;

            // Wait for a short interval
            yield return new WaitForSeconds(blinkInterval);

            // Set the character sprite to be visible
            spriteRenderer.enabled = true;

            // Wait for a short interval
            yield return new WaitForSeconds(blinkInterval);

            // Check if the total blink duration has been reached
            blinkDuration -= blinkInterval * 2; // Subtracting twice the interval because we have two waits in each iteration

            if (blinkDuration <= 0)
            {
                // If the total blink duration is reached, exit the loop
                break;
            }
        }
        iFrames = false;
    }

    public void ApplyKnockback(Vector2 knockbackDirection)
    {
        rb.velocity = new Vector2(knockbackDirection.normalized.x * knockbackForce, jumpForce/2);

        inputDisabled = true;
        StartCoroutine(LookForGround());
    }
}