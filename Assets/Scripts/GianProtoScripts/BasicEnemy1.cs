using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy1 : MonoBehaviour
{
    /*Script that will be attached to each basic enemy 1 gameobject throughout the game. Holds individual values for damage, resistance, health & movement speed and feeds that into it's own instance of the BasicEnemyClass. */

    public float damage;
    public float resistance;
    public float health;
    public float movementSpeed;

    private BasicEnemyClass basicEnemyClass;
    
    void Awake()
    {
        InitialiseClassInstance();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(health);        
    }

    //Initialises an instance of the Basic Enemy Class, feeding it values for damage, resistance, health, movespeed as the constructor requires.
    private void InitialiseClassInstance()
    {
        basicEnemyClass = new BasicEnemyClass(damage, resistance, health, movementSpeed);
    }

    public void TakeDamage(float playerDamage)
    {
        // Inflict damage based on damage value taken from call
        health -= playerDamage;

        // If enemy health drops below zero
        if (health <= 0)
        {
            // Death animation

            // Invoke death for animation duration or call it when animation finishes
            EnemyDead();
        }
    }

    void EnemyDead()
    {
        // Destroy Gameobject
        Destroy(gameObject);
    }
}
