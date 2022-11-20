using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
  private StateMachine meleeStateMachine;

  [SerializeField] public ParticleSystem hitFX;
  [SerializeField] public Collider2D hitbox;
  [SerializeField] CameraShake cameraShake;
  public bool impactShake;

  private void Start()
  {
    meleeStateMachine = GetComponent<StateMachine>();
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
    {
        meleeStateMachine.SetNextState(new GroundComboEntryState());
    }

    if (impactShake)
    {
        StartCoroutine(cameraShake.Noise(1.5f, 1f, 0.1f));
        impactShake = false;
    }
  }



}
