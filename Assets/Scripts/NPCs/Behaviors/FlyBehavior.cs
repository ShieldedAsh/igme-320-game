using UnityEngine;

public class FlyBehavior : NPCBaseBehavior
{
    Vector2 destination;

    protected override void Start()
    {
        base.Start();
        destination = new Vector2(Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 5.0f));
        if (actions.Count <= 0)
        {
            actions.Add(new Move(this.transform, 1.0f));
        }
    }

    protected override float Act()
    {
        currentAction = actions[0];
        if (currentAction is Move)
        {
            StartCoroutine(((Move)currentAction).MoveTo(destination, currentAction.Duration));
            destination = new Vector2(Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 5.0f));
        }
        return currentAction.Duration;
    }
}
