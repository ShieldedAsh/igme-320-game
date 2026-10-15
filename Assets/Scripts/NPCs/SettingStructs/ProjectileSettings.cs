using UnityEngine;

public struct ProjectileSettings
{
    int count;
    float scale;
    float speed;
    float lifetime;
    Transform projectileSpawnLocation;

    public int Count { get => count; }
    public float Scale { get => scale; }
    public float Speed { get => speed; }
    public float Lifetime { get => lifetime; }
    public Transform SpawnLocation { get => projectileSpawnLocation; }

    public ProjectileSettings(int _count, float _scale, float _speed, float _lifetime, Transform _projectileSpawnLocation = null) : this()
    {
        count = _count;
        scale = _scale;
        speed = _speed;
        lifetime = _lifetime;
        projectileSpawnLocation = _projectileSpawnLocation;
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

    public void SetSpawnLocation(Transform location)
    {
        projectileSpawnLocation = location;
    }
}