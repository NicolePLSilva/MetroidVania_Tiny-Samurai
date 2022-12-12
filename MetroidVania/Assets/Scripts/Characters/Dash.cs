using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer trail;

    private MeleeAttack melee;
    private Jump jump;
    private Walk walk;

    Rigidbody2D rb;

    Animator animator;   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>(); 
        animator = GetComponent<Animator>();

        melee = GetComponent<MeleeAttack>();
        jump = GetComponent<Jump>();
        walk = GetComponent<Walk>();
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (Input.GetButtonDown("Fire3") && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
         rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        animator.SetBool("IsDashing", true);
        DisableControl();
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        animator.SetBool("IsDashing", false);
        rb.gravityScale = originalGravity;
        isDashing = false;
        EnableControl();
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void DisableControl()
    {
        melee.enabled = false;
        jump.enabled = false;
        walk.enabled = false;
    }

    private void EnableControl()
    {
        melee.enabled = true;
        jump.enabled = true;
        walk.enabled = true;
    }

}
