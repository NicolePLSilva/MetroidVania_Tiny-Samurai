using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [SerializeField] float runSpeed = 6f;
    [Header("FX")]
    [SerializeField] ParticleSystem surfaceContactParticles;

    float horizontal;
    float scaleX;

    bool canMove = true;
    public bool CanMove{ get => canMove; set => canMove = value;}

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scaleX = 1f;
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
        Flip();
    }

     private void WalkingMovement()
    {
        Vector2 playerVelocity = new Vector2(horizontal * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        
    }

    void Flip()
    {
        
        if(horizontal > 0)
        {
            scaleX = 1f;
        }else if (horizontal < 0)
        {
            scaleX = -1f;
        }
        transform.localScale = new Vector3(scaleX , 1f, 1f);
        PlayDustParticle();
    }

    private void PlayDustParticle()
    {
        surfaceContactParticles.Play();
    }

}
