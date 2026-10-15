using System;
using UnityEngine;

public class ChargerBehavior : NPCBaseBehavior
{
    [SerializeField]
    GameObject target;

    protected override void Start()
    {
        base.Start();
        actions.Add(new Move(this.transform));
        currentAction = actions[0];
    }

    protected override float Act()
    {
        try
        {
            StartCoroutine(ConvertActionTo<Move>(currentAction).ChargeInto(0.5f, target, 4.0f));
        }
        catch (TypeAccessException e)
        {
            Debug.Log(e);
        }
        return currentAction.Duration;
    }
}
