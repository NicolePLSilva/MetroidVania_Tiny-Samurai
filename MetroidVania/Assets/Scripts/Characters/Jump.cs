using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Run Settings")]
    [Range(1,20)]
    [SerializeField] float jumpVelocity;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    [SerializeField] float distanceToTheSurface = 0.05f;
    [SerializeField] LayerMask mask;

    [Header("Wall Jump Settings")]
    [SerializeField] float wallJumpVelocity = 6f;
    [SerializeField] Vector2 wallJumpDirection;
    [SerializeField] float wallJumpDelay = .8f;

    [Header("Camera")]
    [SerializeField] ParticleSystem surfaceContactParticles;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float frequency = 0.5f;
    [SerializeField] float duration = 0.1f;

    bool jumpRequest;
    bool grounded;
    bool onLeftWall;
    bool onRightWall;
    bool leftWallJumpRequest;
    bool rightWallJumpRequest;
    float currentWallTouched = 1f;

    float coyoteTime = 0.2f;
    float coyoteTimeCounter;
    float jumpBufferTime = 0.2f;
    float jumpBufferCounter;

    float inTheAir;

    Rigidbody2D rb;
    Vector2 playerSize;
    Vector2 boxSize;
 
    

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<BoxCollider2D>().size;

        boxSize = new Vector2(playerSize.x, distanceToTheSurface);
    }


    void Update()
    {
        JumpInput();
    }

    private void FixedUpdate() 
    {
        if(grounded)
        {
            coyoteTimeCounter = coyoteTime;
            if (inTheAir > 0.7f)
            {
                StartCoroutine(cameraShake.Noise(amplitude * inTheAir +1, frequency * inTheAir +1, duration));
            }
                inTheAir = 0f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            inTheAir += Time.deltaTime; 
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

        StartCoroutine(cameraShake.Noise(amplitude, frequency, duration));
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
    {
        Vector2 force = new Vector2(wallJumpVelocity * wallJumpDirection.x * direction, wallJumpVelocity * wallJumpDirection.y );
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(cameraShake.Noise(amplitude, frequency, duration));
        StartCoroutine(StopMovement());

        leftWallJumpRequest = false;
        onLeftWall = false;
        rightWallJumpRequest = false;
        onRightWall = false;
    }

    IEnumerator StopMovement()
    {
        GetComponent<Walk>().CanMove = false;
        yield return new WaitForSeconds(wallJumpDelay);
        GetComponent<Walk>().CanMove = true;
    }

    private void CheckOverlap()
    {
        Vector2 boxCenterPoint = (Vector2) transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
        grounded = (Physics2D.OverlapBox(boxCenterPoint, boxSize * 0.7f, 0f, mask) != null );

        Vector2 boxLeft = (Vector2) transform.position + Vector2.left * (playerSize.x + boxSize.x) * 0.3f;
        onLeftWall = (Physics2D.OverlapBox(boxLeft, boxSize * 0.7f, 90f, mask) != null);

        Vector2 boxRight = (Vector2) transform.position + Vector2.right * (playerSize.x + boxSize.x) * 0.3f;
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
