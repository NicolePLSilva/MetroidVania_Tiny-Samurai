using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComboEntryState : MeleeBaseState
{
    public override void OnEnter(StateMachine state)
    {
        base.OnEnter(state);
        
        attackIndex = 1;
        duration = 0.5f;
        animator.SetTrigger("Attack"+1);
        Debug.Log("Player attack "+1+" Fired!");
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
