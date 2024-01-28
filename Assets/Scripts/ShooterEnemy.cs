using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : enemy
{
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootInterval = 3f;
    public float projectileSpeed = 10f;

    private bool isFacingRight = true;
    private float timeSinceLastShot;

    public Animator animator;

    private void Update()
    {
        if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
        else if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }

        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootInterval && Vector3.Distance(player.transform.position, transform.position) < 10)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Shoot()
    {
        animator.SetBool("singing", true);
        Invoke("StopSinging", 1.5f);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<MusicalNote>().rRight = isFacingRight;
        projectile.GetComponent<MusicalNote>().referenceTransform = transform;
    }

    void StopSinging()
    {
        animator.SetBool("singing", false);
    }
}