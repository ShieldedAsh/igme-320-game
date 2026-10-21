using UnityEngine;

#nullable enable
public struct ProjectileSettings
{
    float scale;
    float speed;
    float lifetime;
    GameObject? projectileSpawnLocationObject;
    Vector2 projectileSpawnOffset;

    public float Scale { get => scale; }
    public float Speed { get => speed; }
    public float Lifetime { get => lifetime; }


    public GameObject? LocationObject { get => projectileSpawnLocationObject; }
    public Vector2 SpawnOffset { get => projectileSpawnOffset; }

    public ProjectileSettings(float _scale, float _speed, float _lifetime) : this(_scale, _speed, _lifetime, null, null) {}

    public ProjectileSettings(float _scale, float _speed, float _lifetime, GameObject? _projectileSpawnLocationObject = null) : this(_scale, _speed, _lifetime, _projectileSpawnLocationObject, null) {}

    public ProjectileSettings(float _scale, float _speed, float _lifetime, Vector2? _projectileSpawnOffset = null) : this(_scale, _speed, _lifetime, null, _projectileSpawnOffset) {}

    public ProjectileSettings(float _scale, float _speed, float _lifetime, GameObject? _projectileSpawnLocationObject = null, Vector2? _projectileSpawnOffset = null) : this()
    {
        scale = _scale;
        speed = _speed;
        lifetime = _lifetime;
        projectileSpawnLocationObject = _projectileSpawnLocationObject;
        if (_projectileSpawnOffset != null)
        {
            projectileSpawnOffset = (Vector2)_projectileSpawnOffset;
        }
        else
        {
            projectileSpawnOffset = new Vector2(0, 0);
        }
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

    public void SetLocationObject(GameObject obj)
    {
        projectileSpawnLocationObject = obj;
    }

    public void SetSpawnOffset(Vector2 offset)
    {
        projectileSpawnOffset = offset;
    }
}