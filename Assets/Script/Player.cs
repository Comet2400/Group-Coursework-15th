using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SoundManager soundManager;

   private Animator animator;
    private bool isDashing = false;
    private bool isShooting = false;

    public float speed = 5f; // Adjust this to change the player speed
    public GameObject bulletPrefab; 
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f; 
    public float shootingCooldown = 0.5f;
    private float lastShotTime;
    public int maxHealth = 100; 

    public float immunityDuration = 3f; 
    private bool isImmune = false; // Flag to track the player's immunity status
    private float immunityTimer = 0f; //Timer to track immunity duration 

    public float walkingSoundCooldown = 0.5f;
    private float walkingSoundTimer;

    private int currentHealth;

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

        
        if (horizontalInput != 0 || verticalInput != 0)
        {
            
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
            // Character is idle
            animator.SetInteger("Direction", -1); // Set to idle animation

        }

        //animate dashing
        if (Input.GetKeyDown(KeyCode.X) && isDashing) 
        {
            animator.SetTrigger("Dash");
            isDashing = true;
        }

        if (Input.GetMouseButtonDown(0) && isShooting) // animate shooting
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
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShotTime + shootingCooldown)
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
        // Trigger shooting animation
        animator.SetTrigger("Shoot");

        // Determine bullet direction based on player's animation state
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 direction = Vector2.zero;

        // Set bullet direction based on player's animation state
        if (horizontalInput > 0)
        {
            // Moving right
            direction = Vector2.right;
        }
        else if (horizontalInput < 0)
        {
            // Moving left
            direction = Vector2.left;
        }
        else if (verticalInput > 0)
        {
            // Moving up
            direction = Vector2.up;
        }
        else if (verticalInput < 0)
        {
            // Moving down
            direction = Vector2.down;
        }
        else
        {
            // Default direction (right)
            direction = Vector2.right;
        }

        // Instantiate bullet
        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>(); // Get Bullet component

        // Set bullet direction
        if (bullet != null)
        {
            bullet.SetDirection(direction);
        }

        // Rotate bullet to face its direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
        
        Destroy(gameObject);
        
        Debug.Log("Player died!");
    }

    // Function to handle collision with enemies
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10); 
            Debug.Log("Player damaged!");
        }
    }



    
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
