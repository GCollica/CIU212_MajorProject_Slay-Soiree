using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTotem : MonoBehaviour
{
    public float damageMultiplyer;
    private BasicEnemy1 enemy;

    // Reference to player class script

    public float health; 

    void Awake()
    {
        // Toggle damage increase true

    }


    public void TakeDamage(float damage)
    {
        health = health -= damage;

        if (health <= 0)
        {
            // Toggle damage increase false

            Destroy(gameObject);
        }
    }
}
