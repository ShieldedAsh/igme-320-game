using UnityEngine;

public struct ProjectileSettings
{
    int count;
    float scale;
    float speed;
    float lifetime;

    public int Count { get => count; }
    public float Scale { get => scale; }
    public float Speed { get => speed; }
    public float Lifetime { get => lifetime; }

    public ProjectileSettings(int _count, float _scale, float _speed, float _lifetime) : this()
    {
        count = _count;
        scale = _scale;
        speed = _speed;
        lifetime = _lifetime;
    }

    public void SetCount(int value)
    {
        count = value;
    }

    public void SetScale(float value)
    {
        scale = value;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void SetLifetime(float value)
    {
        lifetime = value;
    }
}