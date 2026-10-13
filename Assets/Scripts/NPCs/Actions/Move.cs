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
    /// Moves the NPC to a destination
    /// </summary>
    /// <param name="destination">The place to move to</param>
    /// <param name="easeType">The type of easing to use while moving. Default is no easing</param>
    /// <returns>A IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator MoveTo(Vector2 destination, EaseType easeType = EaseType.None)
    {
        return MoveTo(destination, actionDuration, easeType);
    }

    /// <summary>
    /// Moves the NPC to a destination
    /// </summary>
    /// <param name="destination">The place to move to</param>
    /// <param name="moveTime">The time it takes to move to destination</param>
    /// <param name="easeType">The type of easing to use while moving. Default is no easing</param>
    /// <returns>A IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator MoveTo(Vector2 destination, float moveTime, EaseType easeType = EaseType.None)
    {
        Vector2 startPos = parent.position;
        float elapsedTime = 0;
        
        if (moveTime > actionDuration)
        {
            SetActionDuration(moveTime);
        }

        while (elapsedTime < moveTime)
        {
            //Changes the easing type
            float t = (elapsedTime / moveTime);
            t = AddEase(t, easeType);
            
            parent.position = Vector2.Lerp(startPos, destination, t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        parent.position = destination;
    }

    /// <summary>
    /// Moves to the target position after a charge-up
    /// </summary>
    /// <param name="chargeUpTime">The amount of time to wait before charging</param>
    /// <param name="target">The target to move to</param> 
    /// <param name="easeType">The type of easing to use while moving. Default is Ease Out</param>
    /// <returns>A IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator ChargeTo(float chargeUpTime, Vector2 target, EaseType easeType = EaseType.EaseOut)
    {
        if (chargeUpTime < 0)
        {
            chargeUpTime = 0.5f;
        }
        float moveTime = actionDuration - chargeUpTime;
        if (moveTime <= 0)
        {
            moveTime = actionDuration;
            SetActionDuration(actionDuration + chargeUpTime);
        }
        yield return new WaitForSeconds(chargeUpTime);
        yield return MoveTo(target, moveTime, easeType);
    }

    //Target has to be a gameObject so it can still know the location after the charge-up
    /// <summary>
    /// Charges into the target's position after a charge-up
    /// </summary>
    /// <param name="chargeUpTime">The amount of time to wait before charging</param>
    /// <param name="target">The target to charge into</param> 
    /// <param name="overshootDistance">How far the NPC will charge past the target</param> 
    /// <param name="easeType">The type of easing to use while moving. Default is Ease Out</param>
    /// <returns>A IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator ChargeInto(float chargeUpTime, GameObject target, float overshootDistance = 2.0f, EaseType easeType = EaseType.EaseOut)
    {
        yield return new WaitForSeconds(chargeUpTime);

        float angle = AngleMath.ToRadians(AngleMath.GetVectorDirection((Vector2)(target.transform.position - parent.transform.position)));
        Vector2 destination = new Vector2(target.transform.position.x + (overshootDistance * Mathf.Cos(angle)), target.transform.position.y + (overshootDistance * Mathf.Sin(angle)));
        yield return ChargeTo(0.0f, destination, easeType);
    }

    //untested
    // Recomending EaseOut for jumping upwards, EaseIn/SmoothStep for jumping down
    private IEnumerator JumpTo(Vector2 end, float moveTime, EaseType easeType = EaseType.EaseOut) {
        //Not actual height, will see if I can make that work
        float height = 1;


        // look into always starting at 0,0 and having end be relative to start, then transforming to original positions
        Vector2 start = parent.transform.position;
        float xDist = end.x - start.x;
        float midY = Mathf.Max(start.y, end.y) + height;
        float a = 2 * (start.y + end.y - 2 * midY);
        float b = -(start.x * (start.y + 3 * end.y) + end.x * (end.y + 3 * start.y) - 4 * midY * (end.x + start.y));
        float c = -(start.x * (start.x * (start.y - end.y) - end.x * (3 * start.y + end.y - 4 * midY)));

        float elapsedTime = 0;
        if (moveTime > actionDuration) 
        {
            SetActionDuration(moveTime);
        }

        while (elapsedTime < moveTime) 
        {
            float t = (elapsedTime / moveTime)
            t = AddEase(t, easeType);
            float x = t * xDist + start.x;

            // y = ax^2 + bx + c
            float y = ((x * x) * (a / (xDist * xDist))) + ((x) * (b / (xDist * xDist))) + ((c / (xDist * xDist)) + start.y); 
            parent.position = new Vector2(x, y);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        parent.position = destination;
    }

    private float AddEase(float t, EaseType easeType) 
    {
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
        return t;
    }
}

/*
*
*
/