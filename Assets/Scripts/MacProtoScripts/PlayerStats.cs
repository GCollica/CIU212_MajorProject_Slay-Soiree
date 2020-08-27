using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float resistance;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(float incomingDamage)
    {
        float resCalculated = (incomingDamage * (1 - (resistance / 100f)));
        health = health -= resCalculated;
    }
}
