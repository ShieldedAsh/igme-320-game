using UnityEngine;
using UnityEngine.UIElements;

public struct MeleeSettings
{
    float lifetime;
    Vector2 scale;

    public float Lifetime { get => lifetime; }
    public Vector2 Scale { get => scale; }

    public MeleeSettings(float _lifetime, Vector2 _scale)
    {
        lifetime = _lifetime;
        scale = _scale;
    }

    public MeleeSettings(float _lifetime, float _xScale = 1.0f, float _yScale = 1.0f) : this(_lifetime, new Vector2(_xScale, _yScale)) {}

    public void SetScale(Vector2 scale)
    {
        this.scale = scale;
    }

    public void SetScale(float xScale, float yScale)
    {
        SetScale(new Vector2(xScale, yScale));
    }

    public void SetXScale(float scale)
    {
        SetScale(new Vector2(scale, this.scale.y));
    }

    public void SetYScale(float scale)
    {
        SetScale(new Vector2(this.scale.x, scale));
    }

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
    }
}
