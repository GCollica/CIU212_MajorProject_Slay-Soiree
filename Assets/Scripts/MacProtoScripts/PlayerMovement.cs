using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Speed stat that changes with items
    public float playerSpeed;
    //Starting speed for player
    public float baseSpeed;

    //Unity auto-generated input script
    private PlayerInput controls;

    //For calculating movement for character
    Vector2 move;
    private Vector2 targetVelocity;
    //Acceleration rate
    public float forceMult;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //Input set up
        controls = new PlayerInput();
        
        //Light attack
        controls.Player.LightAttack.performed += context => LightAttack();
        
        //Movement with left joystick
        controls.Player.Movement.performed += context => move = context.ReadValue<Vector2>();
        controls.Player.Movement.canceled += context => move = Vector2.zero;

        //Heavy attack
        controls.Player.HeavyAttack.performed += context => HeavyAttack();

        //Interact
        controls.Player.Interact.performed += context => Interact();

        //Item
    }

    void FixedUpdate()
    {
        //Assigns "m" to the Vector2 value of the left joystick axes
        Vector2 m = new Vector2(move.x, move.y);
        
        //Sets the velocity to accelerate to
        targetVelocity = m * ((baseSpeed + playerSpeed) * 100) * Time.fixedDeltaTime;
        
        //Calculates the amount of force delivered each frame
        Vector2 force = (targetVelocity - rb.velocity) * forceMult;
        
        //Adds force
        rb.AddForce(force);
    }

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

    void Move(Vector2 direction)
    {
        Debug.Log("Moveing!" + direction);
    }

    //Enables controls when we need them
    void OnEnable()
    {
        controls.Enable();
    }

    //Disables controls when we don't need them
    void OnDisable()
    {
        controls.Disable();
    }
}
