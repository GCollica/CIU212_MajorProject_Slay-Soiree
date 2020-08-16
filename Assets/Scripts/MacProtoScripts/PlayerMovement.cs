using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Player Stats
    private float playerSpeed; 
    #endregion


    //Player Movement Speed
    public float basemoveSpeed;

    public Rigidbody2D rb;

    Vector2 movement;

    void Update()
    {
        //Sets horizontal movement on the y axis and vertical movement 
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //if(Input.GetButtonDown)
    }

    void FixedUpdate()
    {
        rb.velocity = movement * ((basemoveSpeed + playerSpeed) * 100) * Time.fixedDeltaTime;
    }
}
