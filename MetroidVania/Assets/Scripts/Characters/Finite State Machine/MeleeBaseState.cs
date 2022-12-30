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
    private ParticleSystem hitEffect; //Hit Effect to spawn on the afflicted Enemy

    private float attackPressTimer = 0f;

    private NewControls myInput;
    public override void OnEnter(StateMachine state)
    {
        myInput = new NewControls();
        myInput.Player.attack.performed += ctx => AttackPressTimer();
        base.OnEnter(state);
        animator = GetComponent<Animator>();

        colliderDamaged = new List<Collider2D>();
        hitCollider = GetComponent<MeleeAttack>().hitbox;
        hitEffect = GetComponent<MeleeAttack>().hitFX;
        myInput.Enable();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        attackPressTimer -= Time.deltaTime;

        if (animator.GetFloat("Weapon.Active") > 0f)
        {
            Attack();
        }

        if (animator.GetFloat("AttackWindow.Open") > 0f && attackPressTimer > 0f)
        {
            shouldCombo = true;
        }
    }

    void AttackPressTimer()
    {
        attackPressTimer = 2f;
        Debug.Log("Attack press timer");
    }

    public override void OnExit()
    {
        base.OnExit();
        myInput.Disable();
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
                    HitEffectProcess(collidersToDamage, i);

                    colliderDamaged.Add(collidersToDamage[i]);
                    if (collidersToDamage[i] != null)
                    {
                        DealDamage(collidersToDamage, i);
                    }
                }
            }
        }
    }

    private void DealDamage(Collider2D[] target, int i)
    {
        int _damage = GetComponent<MeleeAttack>().damage;
        if (target[i].GetComponent<Health>().IsDead)
        {
            colliderDamaged.Remove(target[i]);
            return;
        }
        target[i].GetComponent<Health>().TakeDamage(_damage);
    }

    private void HitEffectProcess(Collider2D[] collidersToDamage, int i)
    {
        Transform t = collidersToDamage[i].transform;
        hitEffect.transform.position = t.position;
        hitEffect.Play();

        GetComponent<MeleeAttack>().impactShake = true;
    }
}
