using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName =  "Weapon")]
public class WeaponsSO : ScriptableObject
{
    public string weaponName;

    public Sprite weaponSprite;

    public int damage;
    public float attackSpeed;
    public float attackRange;
    public int cost;
}
