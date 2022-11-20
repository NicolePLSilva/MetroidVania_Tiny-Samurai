using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinisherState : MeleeBaseState
{
     public override void OnEnter(StateMachine state)
    {
        base.OnEnter(state);
        
        attackIndex = 3;
        duration = 0.75f;
        animator.SetTrigger("Attack"+3);
        Debug.Log("Player attack "+3+" Fired!");
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
