using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Unity auto-generated input script | All scripts that require input from the player must reference this script
    private PlayerInputMap controls;
    private PlayerCombat playerCombat;

    void Awake()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();

        // PlayerInputMap must be referenced in this manner to work
        controls = new PlayerInputMap();
        
        // Light attack
        controls.Player.LightAttack.performed += context => LightAttack();

        // Heavy attack
        controls.Player.HeavyAttack.performed += context => HeavyAttack();

        // Interact
        controls.Player.Interact.performed += context => Interact();

        // Item

    }
   
    // What happens for each input press
    void Interact()
    {
        Debug.Log("Interact!");
    }

    void LightAttack()
    {
        Debug.Log("Light Attack!");
        playerCombat.LightAttack();
    }

    void HeavyAttack()
    {
        Debug.Log("Heavy Attack!");
        playerCombat.HeavyAttack();
    }
}
