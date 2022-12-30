using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private List<Collider2D> colliderDamaged;
    private StateMachine meleeStateMachine;
    private bool canAttack = false;
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    [SerializeField] public ParticleSystem hitFX;
    [SerializeField] public Collider2D hitbox;
    [SerializeField] CameraShake cameraShake;

    public bool impactShake;
    public int damage;

    Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();

        colliderDamaged = new List<Collider2D>();

    }

    private void Start()
    {
        impactShake = false;
    }

    private void Update()
    {

        CheckAttack();


    }

    public void CheckAttack()
    {

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack1");
        }
    }

    public void Attack()
    {
        Collider2D[] collidersToDamage = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int colliderCount = Physics2D.OverlapCollider(hitbox, filter, collidersToDamage);


        for (int i = 0; i < colliderCount; i++)
        {
            TeamComponent hitTeamComponent = collidersToDamage[i].GetComponentInChildren<TeamComponent>();
            if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
            {
                HitEffectProcess(collidersToDamage, i);

                // colliderDamaged.Add(collidersToDamage[i]);
                if (collidersToDamage[i] != null)
                {
                    DealDamage(collidersToDamage, i);
                }
            }

        }

    }

    private void DealDamage(Collider2D[] target, int i)
    {
        if (target[i].GetComponent<Health>().IsDead)
        {
            colliderDamaged.Remove(target[i]);
            return;
        }
        target[i].GetComponent<Health>().TakeDamage(damage);
    }

    private void HitEffectProcess(Collider2D[] collidersToDamage, int i)
    {
        Transform t = collidersToDamage[i].transform;
        hitFX.transform.position = t.position;
        hitFX.Play();

        impactShake = true;
        ShakeCam();
    }

    private void ShakeCam()
    {
        if (impactShake)
        {
            StartCoroutine(cameraShake.Noise(1.5f, 1f, 0.1f));
            impactShake = false;
        }
    }
}
