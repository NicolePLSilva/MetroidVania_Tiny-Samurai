using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComboEntryState : MeleeBaseState
{
    public override void OnEnter(StateMachine state)
    {
        base.OnEnter(state);
        
        attackIndex = 1;
        duration = .5f;
        animator.SetTrigger("Attack"+attackIndex);
        Debug.Log("Player attack "+attackIndex+" Fired!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fixedTime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new GroundComboState());
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }
        }
    }
}
