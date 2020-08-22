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
        var playerControllers = FindObjectsOfType<PlayerMovement>();
        var index = playerInput.playerIndex;
        playerMovement = playerControllers.FirstOrDefault(m => m.GetPlayerIndex() == index);
        Debug.Log(index);
    }

    public void OnMove(CallbackContext context)
    {
        playerMovement.SetInputVector(context.ReadValue<Vector2>());
    }
}
