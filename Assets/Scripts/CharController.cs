using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool inputDisabled;
    bool lookingForGround;

    public float knockbackForce = 3f;

    public float doubleTapTimeThreshold = 0.2f; 

    private bool isSKeyPressed = false;
    private float timeSinceSKeyDown = 0f;

    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isGrounded) anim.SetTrigger("HitTheFloor");

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
        if (horizontalInput != 0) anim.SetBool("Walking", true);
        else anim.SetBool("Walking", false);

        anim.SetFloat("VerticalMove", rb.velocity.y);

        DoubleTapDetect();

        if (isGrounded && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            if (isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                gameController.FallThrough();
            }
        }
        else if (isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
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

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

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
            if (health < 1)
            {
                inputDisabled = true;
                gameController.GameOver();
            }
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
        while (true)
        {
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(blinkInterval);

            spriteRenderer.enabled = true;

            yield return new WaitForSeconds(blinkInterval);

            blinkDuration -= blinkInterval * 2; 

            if (blinkDuration <= 0)
            {
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

    private void OnDestroy()
    {
        inputDisabled = true;
        gameController.GameOver();
    }

    void DoubleTapDetect()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isSKeyPressed)
            {
                isSKeyPressed = true;
                timeSinceSKeyDown = Time.time;
            }
            else
            {
                if (Time.time - timeSinceSKeyDown <= doubleTapTimeThreshold)
                {
                    gameController.FallThrough();
                    isSKeyPressed = false; 
                }
                else
                {
                    isSKeyPressed = false;
                    timeSinceSKeyDown = 0f;
                }
            }
        }
    }
}