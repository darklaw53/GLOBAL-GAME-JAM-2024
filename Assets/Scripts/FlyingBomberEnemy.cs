using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBomberEnemy : enemy
{
    public Transform pointA;
    public Transform pointB;
    public GameObject bombPrefab;
    public Transform bombDropPoint;
    public float moveSpeed = 3f;
    public float bombDropInterval = 3f; // Time between bomb drops

    private bool isMovingToB = true;
    private float timeSinceLastBombDrop;

    public float projectileSpeed = 10f;

    private Rigidbody2D rb;
    private float previousVelocityX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousVelocityX = rb.velocity.x;
    }

    private void Update()
    {
        // Move between two points
        MoveBetweenPoints();

        // Flip when reaching each point
        CheckAndFlip();

        // Drop bombs periodically
        DropBombs();
    }

    private void MoveBetweenPoints()
    {
        // Determine the target point
        Transform targetPoint = isMovingToB ? pointB : pointA;

        // Move towards the target point
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
    }

    private void CheckAndFlip()
    {
        if (Mathf.Sign(previousVelocityX) != Mathf.Sign(rb.velocity.x))
        {
            // Horizontal velocity changed sign, execute your function here
            FFlip();
        }


        // Check if the enemy has reached a target point
        if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
        {
            FFlip();
            isMovingToB = true;
        }
        else if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
        {
            FFlip();
            isMovingToB = false;
        }
    }

    private void FFlip()
    {
        // Flip the enemy horizontally
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void DropBombs()
    {
        // Drop bombs at intervals
        timeSinceLastBombDrop += Time.deltaTime;
        if (timeSinceLastBombDrop >= bombDropInterval)
        {
            GameObject projectile = Instantiate(bombPrefab, bombDropPoint.position, Quaternion.identity);
            timeSinceLastBombDrop = 0f;

            //Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            //projectileRb.velocity = new Vector2(0, -projectileSpeed);
        }
    }
}