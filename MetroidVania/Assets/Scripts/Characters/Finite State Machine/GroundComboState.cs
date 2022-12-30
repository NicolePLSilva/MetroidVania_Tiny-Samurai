using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComboState : MeleeBaseState
{
     public override void OnEnter(StateMachine state)
    {
        base.OnEnter(state);
        
        attackIndex = 2;
        duration = .5f;
        animator.SetTrigger("Attack"+attackIndex);
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
