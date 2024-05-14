using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet
    public int damage = 20; // Damage dealt by the bullet

    void Start()
    {
        // Set the bullet's velocity to move forward
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;

        // Destroy the bullet after some time to prevent it from staying in the scene forever
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Get the enemy component from the collided object and deal damage to it
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Destroy the bullet upon collision with an enemy
            Destroy(gameObject);
        }
    }
}

