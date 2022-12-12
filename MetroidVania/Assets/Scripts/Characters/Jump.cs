using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Jump Settings")]
    [Range(1,20)]
    [SerializeField] float jumpVelocity;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    [SerializeField] float distanceToTheSurface = 0.05f;
    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask platformMask;

    [Header("Wall Jump Settings")]
    [SerializeField] float wallJumpVelocity = 6f;
    [SerializeField] Vector2 wallJumpDirection;
    [SerializeField] float wallJumpDelay = 0.8f;

    [Header("Camera Shake Settings")]
    [SerializeField] CameraShake cameraShake;
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float frequency = 0.5f;
    [SerializeField] float duration = 0.1f;

    [Header("FX")]
    [SerializeField] ParticleSystem surfaceContactParticles;
    

    bool jumpRequest;
    bool grounded;
    bool onPlatform;
    bool onLeftWall;
    bool onRightWall;
    bool leftWallJumpRequest;
    bool rightWallJumpRequest;
    float currentWallTouched = 1f;
    bool wallSlideRequest;
    float wallSlideSpeed = -1f;


    float coyoteTime = 0.2f;
    float coyoteTimeCounter;
    float jumpBufferTime = 0.2f;
    float jumpBufferCounter;

    float inTheAir;

    Rigidbody2D rb;
    Vector2 playerSize;
    Vector2 boxSize;
    Animator animator;
 
    

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<BoxCollider2D>().size;
        animator = GetComponent<Animator>();

        boxSize = new Vector2(playerSize.x, distanceToTheSurface);
    }


    void Update()
    {
        JumpInput();
        WallSlideInput();
    }

    private void FixedUpdate() 
    {
        if(grounded || onPlatform)
        {
            coyoteTimeCounter = coyoteTime;
            animator.SetBool("InTheAir", false);
            if (inTheAir > 0.7f)
            {
                StartCoroutine(cameraShake.Noise(amplitude * inTheAir +1, frequency * inTheAir +1, duration));
                
                PlayDustParticle(0f, 0f, 1f, 1f * inTheAir,0.5f * inTheAir);
            }
                inTheAir = 0f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            inTheAir += Time.deltaTime; 
            animator.SetBool("InTheAir", true);
        }



        if(jumpRequest)
        {
            JumpingProcess();
        }
        else if(leftWallJumpRequest)
        {
            WallJump(currentWallTouched);
        }
        else if(rightWallJumpRequest)
        {
            WallJump(-currentWallTouched);
        }
        else
        {
            CheckOverlap();
        }

        if (wallSlideRequest)
        {
            WallSlide();
        }

        VariableJumpHeight();
    }

    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            animator.SetBool("InTheAir", true);
            jumpRequest = true;
            jumpBufferCounter = 0f;
        }
        else if (jumpBufferCounter > 0f && onLeftWall)
        {
            leftWallJumpRequest = true;
            jumpBufferCounter = 0f;
        }
        else if (jumpBufferCounter > 0f && onRightWall)
        {
            rightWallJumpRequest = true;
            jumpBufferCounter = 0f;
        }
       
    }
    private void JumpingProcess()
    {

        //rb.velocity += Vector2.up * jumpVelocity;
        rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        jumpRequest = false;
        grounded = false;
        onPlatform = false;

        StartCoroutine(cameraShake.Noise(amplitude, frequency, duration));

        PlayDustParticle(0f, 0f, 1f, 0f, 0f);
  
      
    }

    private void VariableJumpHeight()
    {
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpMultiplier;
            coyoteTimeCounter = 0f;
        }
        else
        {
            rb.gravityScale = 1f;
        }  
    }

    void WallJump(float direction)
    {   if(wallSlideRequest)
        {
            StartCoroutine(cameraShake.Noise(amplitude, frequency, duration));
        
        
            Vector2 force = new Vector2(wallJumpVelocity * wallJumpDirection.x * direction, wallJumpVelocity * wallJumpDirection.y );
            rb.velocity = Vector2.zero;// parando a velocidade antes de atribuir para evitar acumulo de velocidade
            rb.AddForce(force, ForceMode2D.Impulse);
            transform.localScale = new Vector3(direction , 1f, 1f);
            GetComponent<Walk>().ScaleX = direction;

            PlayDustParticle(direction, 90f, 0.4f, 0f, 0f);
            
            StartCoroutine(StopMovement());

            leftWallJumpRequest = false;
            onLeftWall = false;
            rightWallJumpRequest = false;
            onRightWall = false;
        }
        
    }

    IEnumerator StopMovement()
    {
        GetComponent<Walk>().CanMove = false;
        GetComponent<Walk>().enabled = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(wallJumpDelay);
        rb.gravityScale = originalGravity;
        GetComponent<Walk>().enabled = true;
        GetComponent<Walk>().CanMove = true;
    }

    void WallSlideInput()
    {
        float theDirection = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        if ((onLeftWall || onRightWall) && !grounded && rb.velocity.y < 0 && theDirection != 0f)
        {
            wallSlideRequest = true;
            animator.SetBool("InTheAir", false);
            animator.SetBool("IsSliding", true);
            
        }
        else
        {
            wallSlideRequest = false;
            animator.SetBool("IsSliding", false);
        }
       
    }

     void WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        //rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
        inTheAir = 0f;
    }

    private void PlayDustParticle(float direction, float rotation, float scale, float spherizeDirection, float randomPosition)
    {
        ParticleSystem.ShapeModule dustShape = surfaceContactParticles.shape;
        dustShape.position = new Vector3(-direction * 0.5f, 0f, 0f);
        dustShape.rotation = new Vector3(0f, 0f, rotation);
        dustShape.scale = new Vector3(scale, 0, 1f);
        dustShape.sphericalDirectionAmount = spherizeDirection;
        dustShape.randomPositionAmount = randomPosition;
        surfaceContactParticles.Play();
    }

    private void CheckOverlap()
    {
        Vector2 boxCenterPoint = (Vector2) transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
        grounded = (Physics2D.OverlapBox(boxCenterPoint, boxSize * 0.7f, 0f, mask) != null );
        onPlatform = (Physics2D.OverlapBox(boxCenterPoint, boxSize * 0.7f, 0f, platformMask) != null );

        Vector2 boxLeft = (Vector2) transform.position + Vector2.left * (playerSize.x + boxSize.x) * 0.5f;
        onLeftWall = (Physics2D.OverlapBox(boxLeft, boxSize * 0.7f, 90f, mask) != null);
      
        Vector2 boxRight = (Vector2) transform.position + Vector2.right * (playerSize.x + boxSize.x) * 0.5f;
        onRightWall = (Physics2D.OverlapBox(boxRight, boxSize * 0.7f, 90f, mask) != null);

    }

    private void OnDrawGizmos() 
    {
        Vector2 boxCenter = (Vector2) transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
        Gizmos.DrawCube(boxCenter, boxSize);

        Vector2 boxLeft = (Vector2) transform.position + Vector2.left * (playerSize.x + boxSize.x) * 0.5f;
        Vector2 boxY = new Vector2(boxSize.y,boxSize.x);
        Gizmos.DrawCube( boxLeft, boxY);

        Vector2 boxRight = (Vector2) transform.position + Vector2.right * (playerSize.x + boxSize.x) * 0.5f;
        Gizmos.DrawCube(boxRight, boxY);
    }
    
}
