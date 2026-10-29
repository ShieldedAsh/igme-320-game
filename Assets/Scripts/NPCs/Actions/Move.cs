using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum EaseType {
    None,
    EaseOut,
    EaseIn,
    SmoothStep,
    SmootherStep,
    FunctionDefault
}

public class Move : NPCBaseAction
{
    #region Setup and Construction

    MoveSettings settings;

    public string CurrentSettings
    {
        get
        {
            string output = $"Move Time: {settings.MoveTime}\n" +
                            $"Speed: {settings.Speed}\n" +
                            $"Wait Time: {settings.WaitTime}\n" +
                            $"Overshoot Distance: {settings.OvershootDistance}\n" +
                            $"Locking (X,Y): {settings.Locking}" +
                            $"Ease Type: {settings.EaseType.HumanName()}";
            return output;
        }
    }

    public Move(Transform _parent, Action<float> _incrementAndDelayFunction, float _actionDuration = 1.0f) : base(_parent, _incrementAndDelayFunction, _actionDuration) { settings = new MoveSettings(1.0f, 1.0f, 1.0f); }
    public Move(Transform _parent, Action<float> _incrementAndDelayFunction, MoveSettings _settings, float _actionDuration = 1.0f) : base(_parent, _incrementAndDelayFunction, _actionDuration) { settings = _settings; }

    public void SetSettings(MoveSettings _settings)
    {
        settings = _settings;
    }

    #endregion


    #region Helper Functions

    /// <summary>
    /// Adds the specified easing into movement
    /// </summary>
    /// <param name="_t">The base iteration value</param>
    /// <param name="_easeType">The type of easing to use</param>
    /// <returns>The new iteration value</returns>
    private float AddEase(float _t, EaseType _default, EaseType _easeType)
    {
        EaseType ease = _default;
        if (_easeType != EaseType.FunctionDefault)
        {
            ease = _easeType;
        }
        switch (ease)
        {
            case EaseType.None:
                break;
            case EaseType.EaseOut:
                _t = Mathf.Sin(_t * Mathf.PI * 0.5f);
                break;
            case EaseType.EaseIn:
                _t = 1.0f - Mathf.Cos(_t * Mathf.PI * 0.5f);
                break;
            case EaseType.SmoothStep:
                _t = _t * _t * _t;
                break;
            case EaseType.SmootherStep:
                _t = _t * _t * _t * (_t * (6.0f * _t - 15.0f) + 10.0f);
                break;
            default:
                break;
        }
        return _t;
    }

    /// <summary>
    /// Verifies that _destination is an expected type to get a location from
    /// </summary>
    /// <typeparam name="T">The type of destination to check</typeparam>
    /// <param name="_destination">The destination to check</param>
    /// <param name="_overshootDistance">How far past the destination to offset by</param>
    /// <returns>A point in 2D space</returns>
    private Vector2 GetDestination<T>(T _destination, float _overshootDistance = 0.0f, bool lockX = false, bool lockY = false)
    {
        Vector2 destination = parent.position;
        if (typeof(GameObject).IsAssignableFrom(typeof(T)))
        {
            GameObject _target = (GameObject)Convert.ChangeType(_destination, typeof(T));
            destination = _target.transform.position;
        }
        else if (typeof(Transform).IsAssignableFrom(typeof(T)))
        {
            Transform _target = (Transform)Convert.ChangeType(_destination, typeof(T));
            destination = _target.position;
        }
        else if (typeof(Vector2).IsAssignableFrom(typeof(T)))
        {
            destination = (Vector2)Convert.ChangeType(_destination, typeof(T));
        }
        else if (typeof(Vector3).IsAssignableFrom(typeof(T))) 
        {
            destination = (Vector2)((Vector3)Convert.ChangeType(_destination, typeof(T)));
        }

        if (lockX)
        {
            destination.x = parent.position.x;
        }
        if (lockY)
        {
            destination.y = parent.position.y;
        }
        if (destination != (Vector2)parent.position)
        {
            destination += AngleMath.GetOvershoot((Vector2)parent.position, destination, _overshootDistance);
        }
        return destination;
    }

    #endregion

    #region No Delay Linear Movement

    /// <summary>
    /// Moves the NPC to a destination with a certain speed. Uses saved settings
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator UntimedMove<T>(T _location)
    {
        return UntimedMove<T>(_location, settings.Speed, settings.OvershootDistance, settings.LockingX, settings.LockingY, settings.EaseType);
    }

    /// <summary>
    /// Moves the NPC to a destination with a certain speed
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <param name="_settings">The settings to use</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator UntimedMove<T>(T _location, MoveSettings _settings)
    {
        return UntimedMove<T>(_location, _settings.Speed, _settings.OvershootDistance, _settings.LockingX, _settings.LockingY, _settings.EaseType);
    }

    /// <summary>
    /// Moves the NPC to a destination with a certain speed
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <param name="_speed">The speed at which the NPC moves to _location</param>
    /// <param name="_overshootDistance">How far the NPC will move past the target</param>
    /// <param name="lockX">If true, the x-coordinate will be the same as the parent</param>
    /// <param name="lockY">If true, the y-coordinate will be the same as the parent</param>
    /// <param name="_easeType">The type of easing to use while moving. Default is no easing</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator UntimedMove<T>(T _location, float _speed, float _overshootDistance = 0.0f, bool lockX = false, bool lockY = false, EaseType _easeType = EaseType.FunctionDefault)
    {
        if (_speed == 0)
        {
            parent.position = GetDestination<T>(_location, _overshootDistance, lockX, lockY);
            delayFunction(delayTime);
        }
        else
        {
            yield return TimedMove(_location, ((Vector3)GetDestination<T>(_location, _overshootDistance, lockX, lockY) - parent.position).magnitude / Mathf.Abs(_speed), 0.0f, false, false, _easeType);
        }
        yield return null;    
    }

    /// <summary>
    /// Moves the NPC to a destination over time. Uses saved settings
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator TimedMove<T>(T _location)
    {
        return TimedMove<T>(_location, settings.MoveTime, settings.OvershootDistance, settings.LockingX, settings.LockingY, settings.EaseType);
    }

    /// <summary>
    /// Moves the NPC to a destination over time
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <param name="_settings">The settings to use</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator TimedMove<T>(T _location, MoveSettings _settings) 
    {
        return TimedMove<T>(_location, _settings.MoveTime, _settings.OvershootDistance, _settings.LockingX, _settings.LockingY, _settings.EaseType);
    }

    /// <summary>
    /// Moves the NPC to a destination over time
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <param name="_moveTime">The time it takes to move to _destination</param>
    /// <param name="_overshootDistance">How far the NPC will move past the target</param>
    /// <param name="lockX">If true, the x-coordinate will be the same as the parent</param>
    /// <param name="lockY">If true, the y-coordinate will be the same as the parent</param>
    /// <param name="_easeType">The type of easing to use while moving. Default is no easing</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator TimedMove<T>(T _location, float _moveTime, float _overshootDistance = 0.0f, bool lockX = false, bool lockY = false, EaseType _easeType = EaseType.FunctionDefault)
    {
        Vector2 startPos = parent.position;
        float elapsedTime = 0;
        
        if (_moveTime > actionDuration)
        {
            SetActionDuration(_moveTime);
        }

        while (elapsedTime < _moveTime)
        {
            float t = (elapsedTime / _moveTime);
            t = AddEase(t, EaseType.None, _easeType);
            
            parent.position = Vector2.Lerp(startPos, GetDestination<T>(_location, _overshootDistance, lockX, lockY), t);
            elapsedTime += Time.fixedDeltaTime;

            yield return null;
        }
        parent.position = GetDestination<T>(_location, _overshootDistance, lockX, lockY);
        delayFunction(delayTime);
    }

    #endregion

    #region Delayed Linear Movement

    /// <summary>
    /// Moves the NPC to a destination with a certain speed after _waitTime seconds. Uses saved settings
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator UntimedCharge<T>(T _location)
    {
        return UntimedCharge<T>(settings.WaitTime, _location, settings.Speed, settings.OvershootDistance, settings.LockingX, settings.LockingY, settings.EaseType);
    }

    /// <summary>
    /// Moves the NPC to a destination with a certain speed after _waitTime seconds
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <param name="_settings">The settings to use</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator UntimedCharge<T>(T _location, MoveSettings _settings)
    {
        return UntimedCharge<T>(settings.WaitTime, _location, _settings.Speed, _settings.OvershootDistance, _settings.LockingX, _settings.LockingY, _settings.EaseType);
    }

    /// <summary>
    /// Moves the NPC to a destination with a certain speed after _waitTime seconds
    /// </summary>
    /// <param name="_waitTime">The amount of time to wait before moving</param>
    /// <param name="_location">The target to move to</param> 
    /// <param name="_speed">The speed at which the NPC moves to _location</param>
    /// <param name="_overshootDistance">How far the NPC will charge past the target</param>
    /// <param name="lockX">If true, the x-coordinate will be the same as the parent</param>
    /// <param name="lockY">If true, the y-coordinate will be the same as the parent</param> 
    /// <param name="_easeType">The type of easing to use while moving. Default is Ease Out</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator UntimedCharge<T>(float _waitTime, T _location, float _speed, float _overshootDistance = 0.0f, bool lockX = false, bool lockY = false, EaseType _easeType = EaseType.FunctionDefault)
    {
        float w = Mathf.Max(_waitTime, 0);
        SetActionDuration(w + 0.1f);

        if (_waitTime > 0)
        {
            yield return new WaitForSeconds(_waitTime);
        }

        Vector2 destination = GetDestination<T>(_location, _overshootDistance, lockX, lockY);

        if (_speed != 0)
        {
            float time = ((Vector3)destination - parent.position).magnitude / Mathf.Abs(_speed);
            SetActionDuration(w + time);
            parent.gameObject.GetComponent<NPCBaseBehavior>().OverwriteActionCooldown(time);
            EaseType ease = EaseType.EaseOut;
            if (_easeType != EaseType.FunctionDefault)
            {
                ease = _easeType;
            }
            yield return TimedMove(destination, time, 0.0f, false, false, ease);
        }
        else
        {
            parent.position = destination;
            delayFunction(delayTime);
        }
    }

    /// <summary>
    /// Moves the NPC to a destination over time after _waitTime seconds. Uses saved settings
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator TimedCharge<T>(T _location)
    {
        return TimedCharge<T>(settings.WaitTime, _location, settings.MoveTime, settings.OvershootDistance, settings.LockingX, settings.LockingY, settings.EaseType);
    }

    /// <summary>
    /// Moves the NPC to a destination over time after _waitTime seconds
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <param name="_settings">The settings to use</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator TimedCharge<T>(T _location, MoveSettings _settings)
    {
        return TimedCharge<T>(settings.WaitTime, _location, _settings.MoveTime, _settings.OvershootDistance, _settings.LockingX, _settings.LockingY, _settings.EaseType);
    }

    /// <summary>
    /// Moves the NPC to a destination over time after _waitTime seconds
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_waitTime">The amount of time to wait before charging</param>
    /// <param name="_location">The target to charge to/into</param> 
    /// <param name="_chargeTime">The time it takes to move to _location</param>
    /// <param name="_overshootDistance">How far the NPC will charge past the target</param> 
    /// <param name="lockX">If true, the x-coordinate will be the same as the parent</param>
    /// <param name="lockY">If true, the y-coordinate will be the same as the parent</param>
    /// <param name="_easeType">The type of easing to use while moving. Default is Ease Out</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator TimedCharge<T>(float _waitTime, T _location, float _chargeTime, float _overshootDistance = 0.0f, bool lockX = false, bool lockY = false, EaseType _easeType = EaseType.FunctionDefault)
    {
        float w = Mathf.Max(_waitTime, 0);
        float c = Mathf.Max(_chargeTime, 0);
        if (w + c > actionDuration)
        {
            SetActionDuration(w + c);
        }

        if (_waitTime > 0)
        {
            yield return new WaitForSeconds(_waitTime);
        }

        Vector2 destination = GetDestination<T>(_location, _overshootDistance, lockX, lockY);

        if (_chargeTime > 0)
        {
            EaseType ease = EaseType.EaseOut;
            if (_easeType != EaseType.FunctionDefault)
            {
                ease = _easeType;
            }
            yield return TimedMove(destination, _chargeTime, 0.0f, false, false, ease);
        }
        else
        {
            parent.position = destination;
            delayFunction(delayTime);
        }
    }

    #endregion

    #region Non-Linear Movement

    /// <summary>
    /// Moves to the destination in an arc over time. Uses saved settings
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator JumpTo<T>(T _location)
    {
        return JumpTo<T>(_location, settings.MoveTime, settings.EaseType);
    }

    /// <summary>
    /// Moves to the destination in an arc over time
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <param name="_settings">The settings to use</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator JumpTo<T>(T _location, MoveSettings _settings)
    {
        return JumpTo<T>(_location, _settings.MoveTime, _settings.EaseType);
    }

    /// <summary>
    /// Moves to the destination in an arc over time
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move to</param>
    /// <param name="_moveTime">The time it takes to move to _location</param>
    /// /// <param name="_easeType">The type of easing to use while moving. Default is SmootherStep(up)/SmoothStep(down)</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator JumpTo<T>(T _location, float _moveTime, EaseType _easeType = EaseType.FunctionDefault) {
        Vector2 start = parent.transform.position;
        Vector2 target = GetDestination<T>(_location);
        float xDist = target.x - start.x;
        if (Mathf.Abs(xDist) <= 0.05f)
        {
            yield return null;
        }
        else
        {
            //Not actual height, will see if I can make that work
            float height = 1;

            // look into always starting at 0,0 and having end be relative to start, then transforming to original positions
            float midY = Mathf.Max(start.y, target.y) + height;
            float a = (2.0f * (start.y + target.y - (2.0f * midY))) / (xDist * xDist);
            float b = -1.0f * (((start.x * (start.y + (3.0f * target.y))) + (target.x * (target.y + (3.0f * start.y))) - ((4.0f * midY) * (target.x + start.x))) / (xDist * xDist));
            float c = start.y - ((start.x * (start.x * (start.y - target.y) - (target.x * ((3.0f * start.y) + target.y - (4.0f * midY))))) / (xDist * xDist));

            float elapsedTime = 0;
            if (_moveTime <= 0)
            {
                _moveTime = actionDuration;
            }
            if (_moveTime > actionDuration)
            {
                SetActionDuration(_moveTime);
            }

            while (elapsedTime < _moveTime)
            {
                float t = (elapsedTime / _moveTime);
                if (start.y <= target.y)
                {
                    t = AddEase(t, EaseType.SmootherStep, _easeType);
                }
                else if (start.y > target.y)
                {
                    t = AddEase(t, EaseType.SmoothStep, _easeType);
                }
                float x = (t * (target.x - start.x)) + start.x;

                // y = ax^2 + bx + c
                float y = (a * (x * x)) + (b * x) + c;
                parent.position = new Vector2(x, y);
                elapsedTime += Time.fixedDeltaTime;

                yield return null;
            }
            parent.position = target;
            delayFunction(delayTime);
        }
    }

    /// <summary>
    /// Moves in an arc toward the location. The end position will be the first location it will land on the ground. Otherwise, it will land at _location
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move towards</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator JumpToSafe<T>(T _location)
    {
        return JumpToSafe<T>(_location, settings.MoveTime, settings.EaseType);
    }

    /// <summary>
    /// Moves in an arc toward the location. The end position will be the first location it will land on the ground. Otherwise, it will land at _location
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move towards</param>
    /// <param name="_settings">The settings to use</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator JumpToSafe<T>(T _location, MoveSettings _settings)
    {
        return JumpToSafe<T>(_location, _settings.MoveTime, _settings.EaseType);
    }

    //I might make an even safer version that check all positions from start but that will take more work so I won't do that right now
    //Also I should make it so i don't repeat all this code (I dont feel like it)
    /// <summary>
    /// Moves in an arc toward the location. The end position will be the first location it will land on the ground. Otherwise, it will land at _location
    /// </summary>
    /// <typeparam name="T">The type used for the destination</typeparam>
    /// <param name="_location">The place to move towards</param>
    /// <param name="_moveTime">The time it takes to move to the end location</param>
    /// <param name="_easeType">The type of easing to use while moving. Default is SmootherStep(up)/SmoothStep(down)</param>
    /// <returns>An IEnumerator to run in StartCoroutine()</returns>
    public IEnumerator JumpToSafe<T>(T _location, float _moveTime, EaseType _easeType = EaseType.FunctionDefault)
    {
        Vector2 start = parent.transform.position;
        Vector2 target = GetDestination<T>(_location);
        float xDist = target.x - start.x;
        if (Mathf.Abs(xDist) <= 0.05f)
        {
            yield return null;
        }
        else
        {
            //Not actual height, will see if I can make that work
            float height = 1;

            // look into always starting at 0,0 and having end be relative to start, then transforming to original positions
            float midY = Mathf.Max(start.y, target.y) + height;
            float a = (2.0f * (start.y + target.y - (2.0f * midY))) / (xDist * xDist);
            float b = -1.0f * (((start.x * (start.y + (3.0f * target.y))) + (target.x * (target.y + (3.0f * start.y))) - ((4.0f * midY) * (target.x + start.x))) / (xDist * xDist));
            float c = start.y - ((start.x * (start.x * (start.y - target.y) - (target.x * ((3.0f * start.y) + target.y - (4.0f * midY))))) / (xDist * xDist));

            //Checks to see if the NPC would land on the ground when landing. If it isn't able to find a spot, it will stop at the original position
            bool foundLandingPosition = false;
            float checkingX = target.x;
            while (!foundLandingPosition)
            {
                Vector2 checking = new Vector2(checkingX, (a * (checkingX * checkingX)) + (b * checkingX) + c);
                RaycastHit2D hit = Physics2D.Raycast(checking, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
                if (hit)
                {
                    //has to be touching the ground
                    float dist = hit.distance - (parent.GetComponent<SpriteRenderer>().bounds.size.y / 2);
                    if (dist <= 0)
                    {
                        target = new Vector2(checking.x, checking.y + dist);
                        foundLandingPosition = true;
                    }
                    checkingX += Time.fixedDeltaTime;
                }
                else
                {
                    //if nothing is hit then the NPC has left the arena, it should just return the original position.
                    foundLandingPosition = true;
                }
            }
            float endX = target.x;

            float elapsedTime = 0;
            if (_moveTime <= 0)
            {
                _moveTime = actionDuration;
            }
            if (_moveTime > actionDuration)
            {
                SetActionDuration(_moveTime);
            }

            while (elapsedTime < _moveTime)
            {
                float t = (elapsedTime / _moveTime);
                if (start.y <= target.y)
                {
                    t = AddEase(t, EaseType.SmootherStep, _easeType);
                }
                else if (start.y > target.y)
                {
                    t = AddEase(t, EaseType.SmoothStep, _easeType);
                }
                float x = (t * (target.x - start.x)) + start.x;

                // y = ax^2 + bx + c
                float y = (a * (x * x)) + (b * x) + c;
                parent.position = new Vector2(x, y);
                elapsedTime += Time.fixedDeltaTime;

                yield return null;
            }
            parent.position = target;
            delayFunction(delayTime);
        }
    }

    #endregion
}