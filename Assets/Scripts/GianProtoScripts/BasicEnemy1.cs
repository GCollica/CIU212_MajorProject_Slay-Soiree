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
        
         //Testing Take Damage Function from basicEnemyClass.

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(this.gameObject.name + basicEnemyClass.damage);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(this.gameObject.name + basicEnemyClass.health);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            basicEnemyClass.TakeDamage(basicEnemyClass.damage);
        }
        
    }

    //Initialises an instance of the Basic Enemy Class, feeding it values for damage, resistance, health, movespeed as the constructor requires.
    private void InitialiseClassInstance()
    {
        basicEnemyClass = new BasicEnemyClass(damage, resistance, health, movementSpeed);
    }
}
