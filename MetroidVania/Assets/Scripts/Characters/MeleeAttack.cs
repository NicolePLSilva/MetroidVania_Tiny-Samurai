using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
  private StateMachine meleeStateMachine;
  private bool canAttack = false;
  public bool CanAttack{ get => canAttack; set => canAttack = value;}

  [SerializeField] public ParticleSystem hitFX;
  [SerializeField] public Collider2D hitbox;
  [SerializeField] CameraShake cameraShake;
  
  public bool impactShake;
  public int damage;



  private void Start()
  {
    meleeStateMachine = GetComponent<StateMachine>();
    impactShake = false;
  }

  private void Update()
  {
      
      CheckAttack();

      ShakeCam();
  }

  public void CheckAttack()
  {
      if (GetComponent<TeamComponent>().teamIndex == TeamIndex.Player)
      {
        if (Input.GetMouseButtonDown(0) && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
        {
            meleeStateMachine.SetNextState(new GroundComboEntryState());
        }
      }
      if (GetComponent<TeamComponent>().teamIndex == TeamIndex.Enemy)
      {
        if (canAttack && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
        {
            meleeStateMachine.SetNextState(new GroundComboEntryState());
            Debug.Log("Can Attack? " + canAttack);
        }
      }
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
