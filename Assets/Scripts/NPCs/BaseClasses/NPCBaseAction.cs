using UnityEngine;

public abstract class NPCBaseAction
{
    protected float actionDuration;
    protected Transform parent;

    public NPCBaseAction(Transform _parent, float _actionDuration = 1.0f)
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
