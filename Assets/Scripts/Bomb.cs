using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageRange = 2;

    GameObject player;
    public GameObject explosion;
    public SpriteRenderer sprite;
    public CircleCollider2D circleCollider;
    public Rigidbody2D rigbody2D;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    void Explode()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Check the distance between this object and the player
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // If the player is in range, deal damage and destroy this object
            if (distanceToPlayer <= damageRange)
            {
                DealDamageToPlayer(player);
            }
        }
        StartCoroutine(Explosion());
    }

    private void DealDamageToPlayer(GameObject player)
    {
        player.GetComponent<CharController>().TakeDamage(damageAmount, transform);
    }

    IEnumerator Explosion()
    {
        sprite.enabled = false;
        circleCollider.enabled = false;
        rigbody2D.simulated = false;
        var x = Instantiate(explosion, transform);
        x.transform.localScale = new Vector2(damageRange + transform.localScale.x, damageRange + transform.localScale.x);
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}