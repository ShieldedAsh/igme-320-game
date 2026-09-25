using System;

[Serializable]
public abstract class Action
{
    protected string name;
    // How long it takes for the action to be complete
    protected float actionTime;

    //* I need to figure out if time needs to be incremented
    //private float timeElapsed;
    //public float TimeElapsed {get;}

    /// <summary>
    /// The logic of the action
    /// </summary>
    protected abstract void PerformAction();
}
