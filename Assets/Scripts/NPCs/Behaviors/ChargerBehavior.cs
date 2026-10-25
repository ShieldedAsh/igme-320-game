using System;
using UnityEngine;

public class ChargerBehavior : NPCBaseBehavior
{
    [SerializeField]
    GameObject target;

    protected override void Start()
    {
        base.Start();
        actions.Add(new Move(this.transform, IncrementEventCounterAndWait));
        actions[0].SetDelayTime(0.0f);
        currentAction = actions[0];
    }

    protected override float Act()
    {
        try
        {
            StartCoroutine(ConvertActionTo<Move>(currentAction).TimedCharge<GameObject>(0.5f, target, 1.0f, 2.0f));
        }
        catch (TypeAccessException e)
        {
            Debug.Log(e);
        }
        return currentAction.Duration;
    }
}
