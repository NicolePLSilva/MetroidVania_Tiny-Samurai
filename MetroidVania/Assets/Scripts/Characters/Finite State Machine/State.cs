using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    protected float time { get; set; }
    protected float fixedTime { get; set; }
    protected float lateTime { get; set; }
    public StateMachine stateMachine;
    
    //Used to initialize variables
    public virtual void OnEnter(StateMachine state)
    {
            stateMachine = state;
    }

    //Used to update variables 
    public virtual void OnUpdate()
    {
        time += Time.deltaTime;
    }

    public virtual void OnFixedUpdate()
    {
        fixedTime += Time.deltaTime;
    }

    public virtual void OnLateUpdate()
    {
        lateTime += Time.deltaTime;
    }

    //Used to clean up hanging references and variables
    public virtual void OnExit()
    {

    }

    #region Passthrough Methods

    /// <summary>
    /// Removes a gameObject. component, or asset.
    /// <summary>
    /// <param name="obj">The type of Component to retrieve.</param>

    protected static void destroy(UnityEngine.Object obj)
    {
        UnityEngine.Object.Destroy(obj);
    }

    /// <summary>
    /// Returns the component of type T if the game object has one attached, null if it doesn't.
    /// <summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>

    protected T GetComponent<T>() where T : Component {return stateMachine.GetComponent<T>();}

    /// <summary>
    /// Returns the component of Type <paramref name="type"/> if the game object has one attached, null if it doesn't.
    /// <summary>
    /// <param name="type">The type of cComponent to retrieve.</param>
    /// <returns></returns>

    protected Component GetComponent(System.Type type) {return stateMachine.GetComponent(type);}

    /// <summary>
    /// Returns the component with name <paramref name="type"/> if the game object has one attached, null if it doesn't.
    /// <summary>
    /// <param name="type">The type of cComponent to retrieve.</param>
    /// <returns></returns>

    protected Component GetComponent(string type) { return stateMachine.GetComponent(type);}

    #endregion


}
