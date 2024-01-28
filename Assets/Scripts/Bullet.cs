using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            // Assuming that the 'enemy' script is attached to the same GameObject as the 'enemy' tag
            enemy enemyScript = collision.gameObject.GetComponent<enemy>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(1);
            }
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            hitbox charScript = collision.gameObject.GetComponent<hitbox>();

            if (charScript != null)
            {
                charScript.player.TakeDamage(1, transform);
            }
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("land")) Destroy(gameObject);
    }
}
