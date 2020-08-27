using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTotem : MonoBehaviour
{
    public float damageMultiplyer;

    // Reference to player stats

    private BasicEnemy1 enemy;

    public float health; 

    void Awake()
    {
        // Toggle bool true for damage multiplayer

    }

    public void TakeDamage(float damage)
    {
        health = health -= damage;

        if (health <= 0)
        {
            // Toggle bool false

            Destroy(gameObject);
        }
    }
}
