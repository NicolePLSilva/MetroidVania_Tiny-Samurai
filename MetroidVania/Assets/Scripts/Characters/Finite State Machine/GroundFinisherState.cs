using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinisherState : MeleeBaseState
{
     public override void OnEnter(StateMachine state)
    {
        base.OnEnter(state);
        
        attackIndex = 3;
        duration = .75f;
        animator.SetTrigger("Attack"+attackIndex);
        Debug.Log("Player attack "+attackIndex+" Fired!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fixedTime >= duration)
        {
            stateMachine.SetNextStateToMain();
        }
    }
}
