using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    private PlayerMovement playerMovement;

    private int playerIndex = 0;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public float lightAttackDamage = 3f;
    public float heavyAttackDamage = 5f;

    void FixedUpdate()
    {
        //Vector2 looDir = playerMovement.m - gameObject.position;
    }

    public void LightAttack()
    {
        Debug.Log("Light Attack!");

        // Play attack animation

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name + "with a light attack!");

            var impactEnemy = enemy.GetComponent<BasicEnemy1>();
            var impactTotem = enemy.GetComponent<DamageTotem>();

            if (impactEnemy != null)
            {
                impactEnemy.TakeDamage(lightAttackDamage);
                continue;
            }
            else if (impactTotem != null)
            {
                impactTotem.TakeDamage(lightAttackDamage);
                continue;
            }
        }
    }

    public void HeavyAttack()
    {
        Debug.Log("Heavy Attack!");

        // Play attack animation

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Interactions for each enemy hit by the attack
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name + "with a heavy attack!");

            var impactEnemy = enemy.GetComponent<BasicEnemy1>();
            var impactTotem = enemy.GetComponent<DamageTotem>();

            if (impactEnemy != null)
            {
                impactEnemy.TakeDamage(heavyAttackDamage);
                continue;
            }
            else if (impactTotem != null)
            {
                impactTotem.TakeDamage(heavyAttackDamage);
                continue;
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted!");
    }

    public void ActiveItem()
    {
        Debug.Log("Used active item!");
    }

    public int GetPlayerIndex()
    {
        //Returns the index of the player (Index 0-3/Player 1-4) 
        return playerIndex;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
