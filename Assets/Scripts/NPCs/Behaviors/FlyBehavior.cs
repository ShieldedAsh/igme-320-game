using UnityEngine;

public class FlyBehavior : NPCBaseBehavior
{
    Vector2 destination;
    Coroutine myCoroutine;

    protected override void Start()
    {
        base.Start();
        myCoroutine = null;
        if (projectilePrefabs.Count > 0)
        {
            actions.Add(new Move(this.transform));
        }
    }

    protected override float Act()
    {
        if (destination == null || (Vector2)transform.position == destination )
        {
            destination = new Vector2(Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 5.0f));
        }
        if (CanAct() && myCoroutine == null)
        {
            currentAction = actions[0];
            if (currentAction is Move)
            {
                myCoroutine = StartCoroutine(((Move)currentAction).MoveTo(destination, currentAction.Duration));
            }
        }
        return 1.0f;
    }
}
