using System;
using UnityEngine;

public struct MoveSettings
{
    float moveTime;
    float speed;
    float waitTime;
    float overshootDistance;
    bool lockX;
    bool lockY;
    EaseType easeType;

    public float MoveTime { get => moveTime; }
    public float WaitTime { get => waitTime; }
    public float Speed { get => speed; }
    public float OvershootDistance { get => overshootDistance; }
    public Tuple<bool,bool> Locking { get { return new Tuple<bool, bool>(lockX, lockY); }}
    public bool LockingX { get => lockX; }
    public bool LockingY { get => lockY; }
    public EaseType EaseType { get => easeType; }

    public MoveSettings (float _moveTime, float _speed, float _waitTime, float _overshootDistance = 0.0f, bool _lockX = false, bool _lockY = false, EaseType _easeType = EaseType.FunctionDefault)
    {
        moveTime = _moveTime;
        speed = _speed;
        waitTime = _waitTime;
        overshootDistance = _overshootDistance;
        lockX = _lockX;
        lockY = _lockY;
        easeType = _easeType;
    }

    public void SetMoveTime(float _value)
    {
        moveTime = _value;
    }
    public void SetWaitTime(float _value)
    {
        waitTime = _value;
    }
    public void SetSpeed(float _value)
    {
        speed = _value;
    }
    public void SetOvershootDistance(float _value)
    {
        overshootDistance = _value;
    }
    public void Lock(Tuple<bool, bool> _value)
    {
        lockX = _value.Item1;
        lockY = _value.Item2;
    }

    public void LockX(bool _value)
    {
        lockX = _value;
    }
    public void LockY(bool _value)
    {
        lockY = _value;
    }
    public void SetEaseType(EaseType _value)
    {
        easeType = _value;
    }
}
