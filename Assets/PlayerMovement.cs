using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player Movement Speed
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

    void Update()
    {
        //Movement on x axis
        movement.x = Input.GetAxisRaw("Horizontal");
        //Movement on y axis
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
