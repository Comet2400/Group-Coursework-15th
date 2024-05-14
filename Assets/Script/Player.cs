using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Adjust this to change the player speed
    public GameObject bulletPrefab; // Prefab of the bullet
    public Transform bulletSpawnPoint; // Point where bullets will be spawned
    public float bulletSpeed = 10f; // Speed of the bullet
    public int maxHealth = 100; // Max Health of the player

    public float immunityDuration = 3f; //Duration of immunity after collsion
    private bool isImmune = false; // Flag to track the player's immunity status
    private float immunityTimer = 0f; //Timer to track immunity duration 


    private int currentHealth; // Current health of the player

    void Start()
    {
        currentHealth = maxHealth; // Set the current health to the max health when the game starts
    }

    void Update()
    {
        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
        transform.Translate(movement);


        // Flip player's sprite if moving left
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip horizontally
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Reset to normal scale
        }

        // Shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // Update immunity timer
        if (isImmune)
        {
            immunityTimer -= Time.deltaTime;
            if (immunityTimer <= 0)
            {
                isImmune = false;
            }
        }
    }


    void Shoot()
    {


        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // Calculate direction based on player's orientation
        Vector2 shootDirection = Vector2.right; // Default direction is right
        if (transform.localScale.x < 0) // If player is facing left
        {
            shootDirection = -Vector2.right; // Shoot left
        }
        else if (transform.up.y > 0) // If player is facing up
        {
            shootDirection = Vector2.up; // Shoot up
        }
        else if (transform.up.y < 0) // If player is facing down
        {
            shootDirection = -Vector2.up; // Shoot down
        }

        // Set bullet's velocity to move in the calculated direction with a constant speed
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;

        // Destroy bullet after some time
        Destroy(bullet, 2f);


    }

    // Function to decrease player's health
    void TakeDamage(int damage)
    {
        if (!isImmune)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                // Start immunity cooldown
                isImmune = true;
                immunityTimer = immunityDuration;
            }
        }
    }

    void Die()
    {
        // Destroy the player object
        Destroy(gameObject);
        // Implement actions to take when the player dies, like game over or reset
        Debug.Log("Player died!");
    }

    // Function to handle collision with enemies
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10); //reduce player health by 10 when colliding with an enemy
            Debug.Log("Player damaged!");
        }
    }



    // Function to handle collision with enemies
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // check if the player is immune before taking further damage
            if (!isImmune)
            {
                TakeDamage(10); // Reduce player health by 10 when colliding with an enemy
                Debug.Log("Player damaged!");
            }
        }
    }
}
