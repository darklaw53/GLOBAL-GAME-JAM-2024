using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : enemy
{
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootInterval = 2f; // Time between shots
    public float projectileSpeed = 10f;

    private bool isFacingRight = true;
    private float timeSinceLastShot;

    private void Update()
    {
        // Flip the enemy if the player is behind it
        if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
        else if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }

        // Shoot at intervals
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootInterval)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
    }

    private void Flip()
    {
        // Flip the enemy horizontally
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Shoot()
    {
        // Instantiate a projectile at the firePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Set the projectile's velocity
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = isFacingRight ? new Vector2(projectileSpeed, 0f) : new Vector2(-projectileSpeed, 0f);
    }
}