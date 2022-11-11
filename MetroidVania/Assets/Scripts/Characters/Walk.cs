using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [SerializeField] float runSpeed = 6f;

    float horizontal;

    bool canMove = true;
   

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        WalkInput();
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            WalkingMovement();
        }
    }
   
    private void WalkInput()
    {
        horizontal = Input.GetAxis("Horizontal");
    }

     private void WalkingMovement()
    {
        Vector2 playerVelocity = new Vector2(horizontal * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
    }

}
