using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string customName;
    private State mainStateType;
    public State CurrentState { get; private set; }
    private State nextState;

     private void Awake()
    {
        SetNextStateToMain();
    }

    private void LateUpdate()
    {
        if(CurrentState != null)
        {
            CurrentState.OnLateUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.OnFixedUpdate();
        }
    }

    void Update()
    {
        if (nextState != null)
        {
            SetState(nextState);
        }

        if (CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }

    private void SetState(State newState)
    {
        nextState = null;
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = newState;
        CurrentState.OnEnter(this);
    }

    public void SetNextState(State newState)
    {
        if (newState != null)
        {
            nextState = newState;
        }
    }
   
    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }

    private void OnValidate()
    {
        if (mainStateType == null)
        {
            if (customName == "ComboSystem")
            {
                mainStateType = new IdleCombatState();
            }
        }
    }
}
