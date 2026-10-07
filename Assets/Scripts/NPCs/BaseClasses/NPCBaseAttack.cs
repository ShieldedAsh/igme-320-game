using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class NPCBaseAttack
{
    protected float actionDuration;
    protected Transform parent;

    public NPCBaseAttack(Transform _parent) : this(_parent, 1.0f) { }

    public NPCBaseAttack(Transform _parent, float _actionDuration)
    {
        parent = _parent;
        actionDuration = _actionDuration;
    }

    public float Duration { get => actionDuration; }
}
