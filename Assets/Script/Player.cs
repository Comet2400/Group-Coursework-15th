using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SoundManager soundManager;

   private Animator animator;
    private bool isDashing = false;
    private bool isShooting = false;

    public float speed = 5f; // Adjust this to change the player speed
    public GameObject bulletPrefab; // Prefab of the bullet
    public Transform bulletSpawnPoint; // Point where bullets will be spawned
    public float bulletSpeed = 10f; // Speed of the bullet
    public float shootingCooldown = 0.5f;
    private float lastShotTime;
    public int maxHealth = 100; // Max Health of the player

    public float immunityDuration = 3f; //Duration of immunity after collsion
    private bool isImmune = false; // Flag to track the player's immunity status
    private float immunityTimer = 0f; //Timer to track immunity duration 

    public float walkingSoundCooldown = 0.5f;
    private float walkingSoundTimer;

    private int currentHealth; // Current health of the player

    private Dash dash;

    void Start()
    {
        currentHealth = maxHealth; // Set the current health to the max health when the game starts
        soundManager = FindObjectOfType<SoundManager>();

        dash = GetComponent<Dash>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
        transform.Translate(movement);

        // Playing walk sound when moving
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Character is moving
            if (horizontalInput > 0)
            {
                // Moving right
                animator.SetInteger("Direction", 3);
            }
            else if (horizontalInput < 0)
            {
                // Moving left
                animator.SetInteger("Direction", 2);
            }
            else if (verticalInput > 0)
            {
                // Moving up
                animator.SetInteger("Direction", 1);
            }
            else if (verticalInput < 0)
            {
                // Moving down
                animator.SetInteger("Direction", 0);
            }
        }
        else
        {
            // Character is not moving
            animator.SetInteger("Direction", -1); // Set to idle animation

        }

        //animate dashing
        if (Input.GetKeyDown(KeyCode.X) && isDashing) 
        {
            animator.SetTrigger("Dash");
            isDashing = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isShooting)
        {
            animator.SetTrigger("Shoot");
            isShooting = true;
        }

            //check if enough time passed since last walk sound
            if (Time.time >= walkingSoundTimer) 
            { 
                 soundManager.PlayWalkSound(true);

                //set timer for next walking sound after cooldown
                walkingSoundTimer = Time.time + walkingSoundCooldown;
            }
        

        // Flip player's sprite if moving left
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip horizontally
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Reset to normal scale
        }

        if (Input.GetKeyDown(KeyCode.X)) 
        {
            AttemptDash();
        }


        // Shooting
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastShotTime + shootingCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }

        //if (Input.GetAxis("Joystick 0") && Time.time >= lastShotTime + shootingCooldown)
        //{
        //    Shoot();
        //    lastShotTime = Time.time;
        //}

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

    public void ResetDashFlag()
    {
        isDashing = false;
    }

    public void ResetShootFlag()
    {
        isShooting = false;
    }

    void AttemptDash()
    {

        if (dash != null) 
        {
            dash.AttemptDash();
        }
    }


    void Shoot()
    {


        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

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

        soundManager.PlayShootingSound();
        

        

    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        Debug.Log("Max health increased to: " + maxHealth);
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

        soundManager.PlayCharacterSound();
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
