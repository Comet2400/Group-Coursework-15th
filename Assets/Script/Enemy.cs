using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 50; // Maximum health of the enemy
    public float moveSpeed = 3f; // Speed at which the enemy moves
    public float attackRange = 1.5f; // Range at which the enemy can attack
    public int attackDamage = 10; // Damage dealt by the enemy

    private int currentHealth; // Current health of the enemy
    private Transform player; // Reference to the player's transform
    private bool isPlayerInRange; // Flag to track if the player is in attack range

   
    void Start()
    {
        currentHealth = maxHealth; // Set current health to maximum health when the game starts
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player's transform

    }

    
    void Update()
    {
        if (player != null)
        {

            // Calculate the direction to move towards the player
            Vector3 moveDirection = (player.position - transform.position).normalized;

            // Move the enemy towards the player at a constant speed
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Move towards the player if they are in range
            if (isPlayerInRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                // Attack the player if in range
                if (Vector2.Distance(transform.position, player.position) <= attackRange)
                {
                    AttackPlayer();
                }
            }
        }
    }

    // Function to decrease enemy's health
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroy the enemy object
        Destroy(gameObject);
        // Implement actions to take when the enemy dies, like adding score or dropping items
    }

    // Function to attack the player
    void AttackPlayer()
    {
        // Implement attack logic here, like dealing damage to the player
        Debug.Log("Attacking player!");
    }

    // Function to handle collision with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // Function to handle exiting collision with player
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

}
