using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float startingHealth;
    public int startingGold;

    public WeaponsSO startingWeapon;
    public ArmourSO startingArmour;

    public PlayerClass playerClass;

    void Start()
    {
        InitialiseClassInstance();
    }

    void Update()
    {
        
    }

    private void InitialiseClassInstance()
    {
        playerClass = new PlayerClass(startingHealth, startingArmour.resistance, startingWeapon.lightDamage, startingWeapon.heavyDamage,  startingWeapon.attackRange,startingArmour.movementSpeed, startingGold, startingWeapon, startingArmour);
    }

    public void TakeDamage(float incomingDamage)
    {
        playerClass.TakeCalculatedDamage(incomingDamage);

        if(playerClass.currentHealth <= 0)
        {
            //Kill player;
        }
    }
}
