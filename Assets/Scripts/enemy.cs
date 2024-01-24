using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int health = 3;

    public void TakeDamage(int value)
    {
        health -= value;
        if (health < 1) Destroy(gameObject);
    }
}