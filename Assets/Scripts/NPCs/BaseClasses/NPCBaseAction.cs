using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class NPCBaseAction
{
    protected float actionDuration;
    protected Transform parent;

    public NPCBaseAction(Transform _parent) : this(_parent, 1.0f) { }

    public NPCBaseAction(Transform _parent, float _actionDuration)
    {
        parent = _parent;
        actionDuration = _actionDuration;
    }

    public float Duration { get => actionDuration; }

    public void SetActionDuration(float value)
    {
        actionDuration = value;
    }
}
