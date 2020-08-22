using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Unity auto-generated input script
    private PlayerInputMap controls;   

    void Awake()
    {
        //Input set up
        controls = new PlayerInputMap();
        
        //Light attack
        controls.Player.LightAttack.performed += context => LightAttack();

        //Heavy attack
        controls.Player.HeavyAttack.performed += context => HeavyAttack();

        //Interact
        controls.Player.Interact.performed += context => Interact();

        //Item

    }
   
    //What happens for each input press
    void Interact()
    {
        Debug.Log("Interact!");
    }

    void LightAttack()
    {
        Debug.Log("Light Attack!");
    }

    void HeavyAttack()
    {
        Debug.Log("Heavy Attack!");
    }
}
