using System.Collections;
using UnityEngine;

public enum EaseType {
    None,
    EaseOut,
    EaseIn,
    SmoothStep,
    SmootherStep
}

public class Move : NPCBaseAction
{
    public Move(Transform _parent, float _actionDuration = 1.0f) : base(_parent, _actionDuration) {}

    /// <summary>
    /// A coroutine that moves the NPC to destination after actionDuration
    /// </summary>
    /// <param name="destination">The place to move to</param>
    /// <param name="easeType">The type of easing to use while moving. Default is no easing</param>
    /// <returns>A IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator MoveTo(Vector2 destination, EaseType easeType = EaseType.None)
    {
        return MoveTo(destination, actionDuration, easeType);
    }

    /// <summary>
    /// A coroutine that moves the NPC to destination after moveTime
    /// </summary>
    /// <param name="destination">The place to move to</param>
    /// <param name="moveTime">The time it takes to move to destination</param>
    /// <param name="easeType">The type of easing to use while moving. Default is no easing</param>
    /// <returns>A IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator MoveTo(Vector2 destination, float moveTime, EaseType easeType = EaseType.None)
    {
        Vector2 startPos = parent.position;
        float elapsedTime = 0;
        
        while (elapsedTime < moveTime)
        {
            //Changes the easing type
            float t = (elapsedTime / moveTime);
            switch (easeType)
            {
                case EaseType.None:
                    break;
                case EaseType.EaseOut:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case EaseType.EaseIn:
                    t = 1.0f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;
                case EaseType.SmoothStep:
                    t = t * t * (3.0f - 2.0f * t);
                    break;
                case EaseType.SmootherStep:
                    t = t * t * t * (t * (6.0f * t - 15.0f) + 10.0f);
                    break;
            }
            
            parent.position = Vector2.Lerp(startPos, destination, t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        parent.position = destination;
    }
}
