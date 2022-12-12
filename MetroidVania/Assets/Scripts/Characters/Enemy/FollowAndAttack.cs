using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAndAttack : MonoBehaviour
{
    [SerializeField] Transform raycast;
    [SerializeField] LayerMask raycastMask;
    [SerializeField] float raycastlength;
    [SerializeField] float attackDistance;
    [SerializeField] float moveSpeed;
    [SerializeField] float timer;

    [SerializeField] ParticleSystem hitFX;
    [SerializeField] CameraShake camShake;
    [SerializeField] public Collider2D hitbox;


    RaycastHit2D hit;
    GameObject target;
    Animator animator;
    float distance;
    bool attackMood;
    bool IsPlayerInRange;
    bool cooldown;
    float inTimer;

    float duration = 1;
    float fixedTime;
    int damage = 1;

    Patrol patrol;
   
    private List<Collider2D> colliderDamaged; 
    
    private void Awake()
    {
        inTimer = timer;
        animator = GetComponent<Animator>();
        patrol = GetComponent<Patrol>();
        colliderDamaged = new List<Collider2D>();
    }
    void FixedUpdate()
    {
        hit = Physics2D.Raycast(raycast.position, patrol.Direction, raycastlength, raycastMask);
        Debug.Log("Direction :"+patrol.Direction);
        RaycastDebugger();

        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else if(hit.collider == null)
        {
            IsPlayerInRange = false;
        }

        if(IsPlayerInRange == false)
        {
            patrol.MustPatrol = true;
            patrol.PatrolProcess();
        }
        fixedTime += Time.deltaTime;
    }

    private void EnemyLogic()
    {
        if(target == null)
        {
            return;
        }
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if(distance <= attackDistance && cooldown == false)
        {
            if (fixedTime >= duration)
            {
                patrol.MustPatrol = false;
                //Attack();
                animator.SetTrigger("Attack1");
                if (animator.GetFloat("Weapon.Active") > 0f)
                {
                    Attack();
                }
            }
            
        }

        if (cooldown)
        {
            animator.ResetTrigger("Attack1");
            Cooldown();
        }
    }

    private void Move()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyMeleeAttack1"))
        {
            
           Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
           transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        timer = inTimer;
        attackMood = true;
        animator.SetBool("IsRunning", false);


            if (hitbox)
            {
                hitFX.transform.position = target.transform.position;
                hitFX.Play();
                StartCoroutine(camShake.Noise(0.8f,0.8f, 0.1f));
                target.GetComponent<Health>().TakeDamage(damage);
            }
                
        
        
        
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && cooldown && attackMood)
        {
            cooldown = false;
            timer = inTimer;
        }
    }

    private void StopAttack()
    {
        cooldown = false;
        attackMood = false;
        animator.ResetTrigger("Attack1");
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if(other.gameObject.tag == "Player")
            {
                target = other.gameObject;
                IsPlayerInRange = true;
            }
        }
    }

    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(raycast.position, patrol.Direction * raycastlength, Color.red);
        }
        else if (attackDistance > distance)
        {
             Debug.DrawRay(raycast.position, patrol.Direction * raycastlength, Color.green);
        }
    }

    public void TriggerCooldown()
    {
        cooldown = true;
    }
}
