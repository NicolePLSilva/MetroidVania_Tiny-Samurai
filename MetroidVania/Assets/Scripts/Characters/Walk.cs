using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    private StateMachine meleeStateMachine;
    [SerializeField] float runSpeed = 6f;
    [Header("FX")]
    [SerializeField] ParticleSystem surfaceContactParticles;

    float horizontal;
    float scaleX;
    public float ScaleX{ get => scaleX; set => scaleX = value;}

    bool canMove = true;
    public bool CanMove{ get => canMove; set => canMove = value;}

    Rigidbody2D rb;
    Animator animator;

     private NewControls myInput;

  private void Awake()
  {
      myInput = new NewControls();
     // myInput.Player.attack.performed += ctx => CheckAttack();

  }

//   private void OnEnable()
//   {
//       myInput.Enable();
//   }

//   private void OnDisable()
//   {
//       myInput.Disable();
//   }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scaleX = 1f;
    }

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
             animator.SetBool("IsRunning", true);
        }
        else if (horizontal < 0)
        {
            animator.SetBool("IsRunning", true);
            scaleX = -1f;
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        transform.localScale = new Vector3(scaleX , 1f, 1f);
        PlayDustParticle();
    }

    private void PlayDustParticle()
    {
        surfaceContactParticles.Play();
    }

}
