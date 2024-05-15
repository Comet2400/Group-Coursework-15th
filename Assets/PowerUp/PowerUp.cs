using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int healthIncreaseAmount = 20; // Amount of health to increase when the power-up is picked up

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.IncreaseMaxHealth(healthIncreaseAmount);
                Destroy(gameObject); // Destroy the power-up object after it has been picked up
            }
            else
            {
                Debug.LogWarning("Player component not found on collision.");
            }
        }
    }
}

