using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        //Creates an array of PlayerMovement scripts for each player
        var playerControllers = FindObjectsOfType<PlayerMovement>();

        //Creates variable that holds the index of the player
        var index = playerInput.playerIndex;

        //Takes the first controller to give input and assigns it to index 0, continues till 4 players have joined with the final index "3"
        playerMovement = playerControllers.FirstOrDefault(m => m.GetPlayerIndex() == index);
        Debug.Log(index);
    }

    public void OnMove(CallbackContext context)
    {
        //Takes the input from object this script is attached to (PlayerInputController)
        playerMovement.SetInputVector(context.ReadValue<Vector2>());
    }
}
