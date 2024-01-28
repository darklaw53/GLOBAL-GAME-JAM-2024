using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalNote : MonoBehaviour
{
    public float initialSpeed = 5f;
    public float altitudeToChangeDirection1 = 5f; 
    public float altitudeToChangeDirection2 = 10f;
    public float lerpSpeed = 2f;

    private Rigidbody2D rb;

    public float speed = 5f; 
    public float initialRotationAngle = -45f;

    public bool rRight;

    public Transform referenceTransform;
    Vector2 trueReference;

    public float heightThreshold = 5f, heightDifference;

    public Quaternion fourtyFive, minusFourtyFive, reverseFourtyFive, minusReverceFourtyFive;
    Quaternion targetRotation;

    bool rotating;

    void Start()
    {
        trueReference = referenceTransform.position;

        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody2D>();

        transform.Rotate(Vector3.forward, 80);
        fourtyFive = transform.rotation;

        transform.rotation = targetRotation;
        transform.Rotate(Vector3.forward, -70);
        minusFourtyFive = transform.rotation;

        transform.rotation = targetRotation;
        transform.Rotate(Vector3.forward, 100);
        reverseFourtyFive = transform.rotation;

        transform.rotation = targetRotation;
        transform.Rotate(Vector3.forward, 250);
        minusReverceFourtyFive = transform.rotation;

        transform.rotation = targetRotation;
        if (rRight) transform.Rotate(Vector3.forward, 80);
        else transform.Rotate(Vector3.forward, 100);
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), 0);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        heightDifference = transform.position.y - trueReference.y;

        if (heightDifference > heightThreshold)
        {
            if (rRight) 
            {
                targetRotation = minusFourtyFive;
            }
            else
            {
                targetRotation = minusReverceFourtyFive;
            }
            rotating = true;
        }
        else if (heightDifference < -heightThreshold)
        {
            if (rRight)
            {
                targetRotation = fourtyFive;
            }
            else
            {
                targetRotation = reverseFourtyFive;
            }
            rotating = true;
        }
        else
        {
            rotating = false;
        }

        if (rotating) Rotating();
    }

    void Rotating()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitbox charScript = collision.gameObject.GetComponent<hitbox>();

            if (charScript != null)
            {
                charScript.player.TakeDamage(1, transform);
            }
            Destroy(gameObject);
        }
    }
}
