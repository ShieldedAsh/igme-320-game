using System;
using UnityEngine;

public class JumperBehavior : NPCBaseBehavior
{
    [SerializeField]
    GameObject target;

    protected override void Start()
    {
        base.Start();
        actions.Add(new Move(this.transform, IncrementEventCounterAndWait));
        currentAction = actions[0];
    }

    protected override float Act()
    {
        try
        {
            StartCoroutine(ConvertActionTo<Move>(currentAction).TimedJump<Vector3>(target.transform.position));
        }
        catch (TypeAccessException e)
        {
            Debug.Log(e);
        }
        return currentAction.Duration;
    }
}
