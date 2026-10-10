using UnityEngine;

public class FlyBehavior : NPCBaseBehavior
{
    Vector2 destination;

    protected override void Start()
    {
        base.Start();
        destination = new Vector2(Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 5.0f));
        actions.Add(new Move(this.transform, 1.0f));
        currentAction = actions[0];
    }

    protected override float Act()
    {
        try
        {
            StartCoroutine(ConvertActionTo<Move>(currentAction).MoveTo(destination));
        }
        catch (System.TypeAccessException e)
        {
            Debug.Log(e);
        }
        destination = new Vector2(Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 5.0f));
        return currentAction.Duration;
    }
}
