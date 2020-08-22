using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private int playerIndex = 0;

    //Starting speed for player
    [SerializeField]
    private float baseSpeed;

    //Speed stat that changes with items
    public float playerSpeed;

    //Unity auto-generated input script
    private PlayerInputMap controls;

    //For calculating movement for character
    Vector2 move;
    private Vector2 targetVelocity;
    //Acceleration rat
    public float forceMult;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerInputMap();
    }

    public void SetInputVector(Vector2 direction)
    {
        //Sets the Vector2 value taken from the PlayerInputHandler script
        move = direction;
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

        Move(m);
    }

    public int GetPlayerIndex()
    {
        //Returns the index of the player (Index 0-3/Player 1-4) 
        return playerIndex;
    }

    void Move(Vector2 direction)
    {
        Debug.Log("Moving!" + direction);
    }
}
