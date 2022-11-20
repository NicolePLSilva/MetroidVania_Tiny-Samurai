using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEntryState : State
{
    public override void OnEnter(StateMachine state)
    {
        base.OnEnter(state);

        // State nextState = (player && player.isGrounded())?(State)new MeleeGround():(State)new MeleeAir();
        State nextState = (State)new GroundComboEntryState();
        stateMachine.SetNextState(nextState);

        
    }
}
