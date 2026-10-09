using System.Collections;
using UnityEngine;

public enum MovementEase {
    None,
    EaseOut,
    EaseIn,
    SmoothStep,
    SmootherStep
}

public class Move : NPCBaseAction
{
    Vector2 startLocation;
    Vector2 currentEndLocation;

    public Move(Transform _parent) : this(_parent, 1.0f) {}

    public Move(Transform _parent, float _actionDuration) : base(_parent, _actionDuration)
    {
    }

    public IEnumerator MoveTo(Vector2 destination, float moveTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            parent.position = Vector2.Lerp((Vector2)parent.position, destination, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        parent.position = destination;
        yield return null;
    }

    /*
     * Ease Out = Mathf.sin(t * Math.PI * 0.5)
     * Ease In = 1 - Mathf.cos(t * Math.PI * 0.5)
     * Smooth Step = t * t * (3.0f - 2.0f * t)
     * Smoother Step = t * t * t * (t * (6.0f * t - 15.0f) + 10.0f)
     */
}
