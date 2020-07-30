using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyClass
{
    /*Base class for all basic enemies in the game, holds values for Damage, Resistance, Current Health, Movement Speed. Also includes commonly used functions shared by all common enemies. Specific functions used by each enemy type individually will be housed in their BasicEnemyX scrpit instead.*/

    public float damage;
    public float resistance;
    public float health;
    public float movementSpeed;

    public BasicEnemyClass(float damageInput, float resistanceInput, float healthInput, float movementSpeedInput)
    {
        damage = damageInput;
        resistance = resistanceInput;
        health = healthInput;
        movementSpeed = movementSpeedInput;
    }

    //Calculates the damage taken given an input value against the resistance of the enemy. Then updates current health to accomodate. 
    public void TakeDamage(float incomingDamage)
    {
        float resCalculated = (incomingDamage * (1 - (resistance / 100f)));
        //Debug.Log(resCalculated);
        health -= resCalculated;
    }

}
