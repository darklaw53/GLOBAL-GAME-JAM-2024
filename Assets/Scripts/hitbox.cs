using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : MonoBehaviour
{
    public CharController player;

    private void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "enemy") player.TakeDamage(1, collision.transform);

        if (collision.transform.tag == "Coin")
        { 
            Destroy(collision.gameObject);

            player.gameController.GainCoin();
        }
    }
}
