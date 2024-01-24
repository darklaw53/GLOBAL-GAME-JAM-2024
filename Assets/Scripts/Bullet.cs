using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            // Assuming that the 'enemy' script is attached to the same GameObject as the 'enemy' tag
            enemy enemyScript = collision.gameObject.GetComponent<enemy>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(1);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            CharController charScript = collision.gameObject.GetComponent<CharController>();

            if (charScript != null)
            {
                charScript.TakeDamage(1, transform);
            }
        }
        Destroy(gameObject);
    }
}
