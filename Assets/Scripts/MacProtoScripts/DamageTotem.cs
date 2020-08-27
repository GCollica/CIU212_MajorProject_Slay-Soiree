using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTotem : MonoBehaviour
{
    public float damageMultiplyer;

    private float startingDamage;
    private float increasedDamage;

    private BasicEnemy1 enemy;

    public float health; 

    void Awake()
    {
        enemy = FindObjectOfType<BasicEnemy1>();

        startingDamage = enemy.damage;
        increasedDamage = enemy.damage * damageMultiplyer;  
    }

    void Start()
    {
        enemy.damage = increasedDamage;
    }

    public void TakeDamage(float damage)
    {
        health = health -= damage;

        if (health <= 0)
        {
            enemy.damage = startingDamage;

            Destroy(gameObject);
        }
    }
}
