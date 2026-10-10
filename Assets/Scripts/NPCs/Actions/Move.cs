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
    public Move(Transform _parent) : this(_parent, 1.0f) {}

    public Move(Transform _parent, float _actionDuration) : base(_parent, _actionDuration) {}

    public IEnumerator MoveTo(Vector2 destination, float moveTime)
    {
        Vector2 startPos = parent.position;

        float elapsedTime = 0;
        
        while (elapsedTime < moveTime)
        {
            float t = (elapsedTime / moveTime);
            parent.position = Vector2.Lerp(startPos, destination, t);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }

        parent.position = destination;
    }

    /*
     * Ease Out = Mathf.sin(t * Math.PI * 0.5)
     * Ease In = 1 - Mathf.cos(t * Math.PI * 0.5)
     * Smooth Step = t * t * (3.0f - 2.0f * t)
     * Smoother Step = t * t * t * (t * (6.0f * t - 15.0f) + 10.0f)
     */
}
