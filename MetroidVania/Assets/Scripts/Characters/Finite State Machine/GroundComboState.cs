using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComboState : MeleeBaseState
{
     public override void OnEnter(StateMachine state)
    {
        base.OnEnter(state);
        
        attackIndex = 2;
        duration = 0.5f;
        animator.SetTrigger("Attack"+2);
        Debug.Log("Player attack "+2+" Fired!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fixedTime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new GroundFinisherState());
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }
        }
    }
}
