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
    public Move(Transform _parent) : this(_parent, 1.0f) {}

    public Move(Transform _parent, float _actionDuration) : base(_parent, _actionDuration) {}

    public IEnumerator MoveTo(Vector2 destination, EaseType easeType = EaseType.None)
    {
        return MoveTo(destination, actionDuration, easeType);
    }

    public IEnumerator MoveTo(Vector2 destination, float moveTime, EaseType easeType = EaseType.None)
    {
        Vector2 startPos = parent.position;
        float elapsedTime = 0;
        
        while (elapsedTime < moveTime)
        {
            float t = (elapsedTime / moveTime);
            switch (easeType)
            {
                case EaseType.None:
                    break;
                case EaseType.EaseOut:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
;                   break;
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
