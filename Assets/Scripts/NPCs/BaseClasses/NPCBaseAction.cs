using System;
using UnityEngine;

public abstract class NPCBaseAction
{
    protected float actionDuration;
    protected Transform parent;
    protected Action<float> delayFunction;
    protected float delayTime;

    public NPCBaseAction(Transform _parent, Action<float> _incrementAndDelayFunction, float _actionDuration = 1.0f, float _delayTime = 0.25f)
    {
        parent = _parent;
        delayFunction = _incrementAndDelayFunction;
        actionDuration = _actionDuration;
        delayTime = _delayTime;
    }

    public float Duration { get => actionDuration; }

    public void SetActionDuration(float _value)
    {
        if (_value < 0)
        {
            actionDuration = 0;
            return;
        }
        actionDuration = _value;
    }

    public void SetDelayTime(float _value)
    {
        delayTime = _value;
    }
}
