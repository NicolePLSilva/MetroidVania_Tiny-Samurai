using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
{
    public float duration;//How long this state should be active for before moving on
    protected Animator animator; 
    protected bool shouldCombo;//check wheter or not the next attack in the sequence should be played or not
    protected int attackIndex;

   
    protected Collider2D hitCollider; //The cached hit collider 
    private List<Collider2D> colliderDamaged; //Cached already struk objects of said attack to avoid overlapping attacks on same target
    private ParticleSystem hitEffectPrefab; //Hit Effect to spawn on the afflicted Enemy
    // private bool cameraShake; 

    private float attackPressTimer = 0;
    public override void OnEnter(StateMachine state)
    {
        base.OnEnter(state);
        animator = GetComponent<Animator>();

        colliderDamaged = new List<Collider2D>();
        hitCollider = GetComponent<MeleeAttack>().hitbox;
        hitEffectPrefab = GetComponent<MeleeAttack>().hitFX;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        attackPressTimer -= Time.deltaTime;

        if (animator.GetFloat("Weapon.Active") > 0f)
        {
            Attack();
        }

        if (Input.GetMouseButtonDown(0))
        {
            attackPressTimer = 0.5f; 
        }

        if (animator.GetFloat("AttackWindow.Open") > 0f && attackPressTimer > 0)
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected void Attack()
    {
        Collider2D[] collidersToDamage = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int colliderCount = Physics2D.OverlapCollider(hitCollider, filter, collidersToDamage);
        for (int i = 0; i < colliderCount; i++)
        {
            if (!colliderDamaged.Contains(collidersToDamage[i]))
            {
                TeamComponent hitTeamComponent = collidersToDamage[i].GetComponentInChildren<TeamComponent>();

                if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
                {
                   //GameObject.Instantiate(hitEffectPrefab, collidersToDamage[i].transform);
                    Transform t = collidersToDamage[i].transform;
                    hitEffectPrefab.transform.position = t.position;
                    hitEffectPrefab.Play();
                    GetComponent<MeleeAttack>().impactShake = true;
                   
                    Debug.Log("Enemy has taken: " + attackIndex+" Damage");
                    colliderDamaged.Add(collidersToDamage[i]);
                }
            }
        }
    }   
}
