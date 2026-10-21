using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * To Do:
 * Emit X-Sequence (spawn delay between projectiles)
 */

public class FireProjectiles : NPCBaseAction
{
    #region Construction and Assignment   

    GameObject projectilePrefab;
    ProjectileSettings settings;
    
    public GameObject ProjectilePrefab { get => projectilePrefab; }
    public string CurrentSettings {
        get {
            string output = $"Projectile Scale: {settings.Scale}x Original\n" +
                            $"Projectile Speed: {settings.Speed}\n" +
                            $"Projectile Lifetime: {settings.Lifetime}\n";
            if (settings.LocationObject != null)
            {
                output +=   $"Projectile Spawn Object: {settings.LocationObject.name}\n";
            }
            output +=       $"Projectile Spawn Offset: {settings.SpawnOffset}";
            return output;
        }
    }
    
    public FireProjectiles(GameObject _projectilePrefab, Transform _parent, float _actionDuration = 1.0f) : base(_parent, _actionDuration) 
    { 
        projectilePrefab = _projectilePrefab;
        settings = new ProjectileSettings(1.0f, 1.0f, 1.0f);
    }
    public FireProjectiles(GameObject _projectilePrefab, Transform _parent, ProjectileSettings _settings, float _actionDuration = 1.0f) : base(_parent, _actionDuration) 
    { 
        projectilePrefab = _projectilePrefab;
        settings = _settings;
    }

    public void SetProjectile(GameObject _prefab)
    {
        projectilePrefab = _prefab;
    }

    public void SetSettings(ProjectileSettings _settings)
    {
        settings = _settings;
    }

    #endregion


    #region Emit Projectile
    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="_fireAngle">The angle to fire towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(float _fireAngle = 0.0f)
    {
        return CreateProjectile(settings.Scale, settings.Speed, settings.Lifetime, settings.LocationObject, settings.SpawnOffset, _fireAngle);
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_fireAngle">The angle to fire towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(float _scale, float _speed, float _lifetime, float _fireAngle = 0.0f)
    {
        return CreateProjectile(_scale, _speed, _lifetime, null, Vector2.zero, _fireAngle);
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_direction">The direction to fire towards</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(float _scale, float _speed, float _lifetime, Vector2 _direction)
    {
        return CreateProjectile(_scale, _speed, _lifetime, null, Vector2.zero, AngleMath.GetVectorDirection(_direction));
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="_settings">The projectile settings</param>
    /// <param name="_fireAngle">The angle to fire towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(ProjectileSettings _settings, float _fireAngle = 0.0f)
    {
        return CreateProjectile(_settings.Scale, _settings.Speed, _settings.Lifetime, _settings.LocationObject, _settings.SpawnOffset, _fireAngle);
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="_settings">The projectile settings</param>
    /// <param name="_direction">The direction to fire towards</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(ProjectileSettings _settings, Vector2 _direction)
    {
        return CreateProjectile(_settings.Scale, _settings.Speed, _settings.Lifetime, _settings.LocationObject, _settings.SpawnOffset, AngleMath.GetVectorDirection(_direction));
    }
    #endregion

    #region Emit Circle
    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="_fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject>EmitCircle(float _fireAngleOffset = 0.0f, int _count = 1)
    {
        return EmitCircle(settings.Scale, settings.Speed, settings.Lifetime, settings.LocationObject, settings.SpawnOffset, _fireAngleOffset, _count);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="_settings">The projectile settings</param>
    /// <param name="_fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject>EmitCircle(ProjectileSettings _settings, float _fireAngleOffset = 0.0f, int _count = 1)
    {
        return EmitCircle(_settings.Scale, _settings.Speed, _settings.Lifetime, settings.LocationObject, settings.SpawnOffset, _fireAngleOffset, _count);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="_settings">The projectile settings</param>
    /// <param name="_directionOffset">The direction to offset the start direction by</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(ProjectileSettings _settings, Vector2 _directionOffset, int _count = 1)
    {
        return EmitCircle(_settings.Scale, _settings.Speed, _settings.Lifetime, _settings.LocationObject, _settings.SpawnOffset, AngleMath.GetVectorDirection(_directionOffset), _count);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_directionOffset">The direction to offset the start direction by</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(float _scale, float _speed, float _lifetime, Vector2 _directionOffset, int _count = 1)
    {
        return EmitCircle(_scale, _speed, _lifetime, null, Vector2.zero, AngleMath.GetVectorDirection(_directionOffset), _count);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(float _scale, float _speed, float _lifetime, float _fireAngleOffset = 0.0f, int _count = 1)
    {
        return EmitCircle(_scale, _speed, _lifetime, null, Vector2.zero, _fireAngleOffset, _count);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_locationObject">The GameObject that will be used as the origin point of spawned projectiles. Null value will default to "Parent" position</param>
    /// <param name="_spawnOffset">The offset from the spawn point, Default is (0,0)</param>
    /// <param name="_fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    private List<GameObject> EmitCircle(float _scale, float _speed, float _lifetime, GameObject _locationObject, Vector2 _spawnOffset, float _fireAngleOffset, int _count)
    {
        List<GameObject> projectiles = new List<GameObject>();
        if (_count <= 1)
        {
            projectiles.Add(CreateProjectile(_scale, _speed, _lifetime, _locationObject, _spawnOffset, _fireAngleOffset));
            return projectiles;
        }

        float deltaAngle = 360.0f / (float)_count;
        float currentAngle = _fireAngleOffset;

        for (int i = 1; i <= _count; i++)
        {
            projectiles.Add(CreateProjectile(_scale, _speed, _lifetime, _locationObject, _spawnOffset, currentAngle));
            currentAngle = currentAngle + deltaAngle;
        }
        return projectiles;
    }
    #endregion

    #region Emit Cross
    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="_fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(float _fireAngleOffset = 0.0f)
    {
        return EmitCircle(4);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(float _scale, float _speed, float _lifetime, float _fireAngleOffset = 0.0f)
    {
        return EmitCircle(_scale, _speed, _lifetime, _fireAngleOffset, 4);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_directionOffset">The direction to offset the direction angle by</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(float _scale, float _speed, float _lifetime, Vector2 _directionOffset)
    {
        return EmitCircle(_scale, _speed, _lifetime, _directionOffset, 4);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="_settings">The projectile settings</param>
    /// <param name="_fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(ProjectileSettings _settings, float _fireAngleOffset = 0.0f)
    {
        return EmitCircle(_settings, _fireAngleOffset, 4);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="_settings">The projectile settings</param>
    /// <param name="_directionOffset">The direction to offset the direction angle by</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(ProjectileSettings _settings, Vector2 _directionOffset)
    {
        return EmitCircle(_settings, _directionOffset, 4);
    }
    #endregion

    #region Emit Arc
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_arcAngle">The width of the arc in degrees</param>
    /// <param name="_fireAngle">The angle to fire towards in degrees</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float _arcAngle, float _fireAngle = 0.0f, int _count = 1)
    {
        return CreateProjectileArc(_arcAngle, settings.Scale, settings.Speed, settings.Lifetime, settings.LocationObject, settings.SpawnOffset, _fireAngle, _count);
    }

    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="_arcAngle">The width of the arc in degrees</param>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_fireAngle">The angle to fire towards in degrees</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float _arcAngle, float _scale, float _speed, float _lifetime, float _fireAngle = 0.0f, int _count = 1)
    {
        return CreateProjectileArc(_arcAngle, _scale, _speed, _lifetime, null, Vector2.zero, _fireAngle, _count);
    }

    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="_arcAngle">The width of the arc in degrees</param>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_direction">The direction to fire towards</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float _arcAngle, float _scale, float _speed, float _lifetime, Vector2 _direction, int _count = 1)
    {
        return CreateProjectileArc(_arcAngle, _scale, _speed, _lifetime, null, Vector2.zero, AngleMath.GetVectorDirection(_direction), _count);
    }


    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="_arcAngle">The width of the arc in degrees</param>
    /// <param name="_settings">The projectile settings</param>
    /// <param name="_fireAngle">The angle to fire towards in degrees</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float _arcAngle, ProjectileSettings _settings, float _fireAngle = 0.0f, int _count = 1)
    {
        return CreateProjectileArc(_arcAngle, _settings.Scale, _settings.Speed, _settings.Lifetime, _settings.LocationObject, _settings.SpawnOffset, _fireAngle, _count);
    }

    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="_arcAngle">The width of the arc in degrees</param>
    /// <param name="_settings">The projectile settings</param>
    /// <param name="_direction">The direction to fire towards</param>
    /// <param name="_count">The amount of projectiles to fire</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float _arcAngle, ProjectileSettings _settings, Vector2 _direction, int _count = 1)
    {
        return CreateProjectileArc(_arcAngle, _settings.Scale, _settings.Speed, _settings.Lifetime, _settings.LocationObject, _settings.SpawnOffset, AngleMath.GetVectorDirection(_direction), _count);
    }
    #endregion

    #region Projectile Creation
    /// <summary>
    /// Instantiates projectiles in an arc and adds them to projectileList
    /// </summary>
    /// <param name="_arcAngle">The width of the arc in degrees</param>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_locationObject">The GameObject that will be used as the origin point of spawned projectiles. Null value will default to "Parent" position</param>
    /// <param name="_spawnOffset">The offset from the spawn point, Default is (0,0)</param>
    /// <param name="_fireAngle">The angle to fire towards in degrees</param>
    /// <param name="_count">The amount of projectiles to instantiate</param>
    /// <returns>A list of instantiated projectiles</returns>
    private List<GameObject> CreateProjectileArc(float _arcAngle, float _scale, float _speed, float _lifetime, GameObject _locationObject, Vector2 _spawnOffset, float _fireAngle = 0.0f, int _count = 1)
    {
        List<GameObject> projectiles = new List<GameObject>();
        if (_count <= 1)
        {
            projectiles.Add(CreateProjectile(_scale, _speed, _lifetime, _locationObject, _spawnOffset, _fireAngle));
            return projectiles;
        }
        float deltaAngle = _arcAngle / (_count - 1.0f);
        float startAngle = _fireAngle + (-_arcAngle / 2.0f);
        float currentAngle = startAngle;
        for (int i = 1; i <= _count; i++)
        {
            projectiles.Add(CreateProjectile(_scale, _speed, _lifetime, _locationObject, _spawnOffset, currentAngle));
            currentAngle += deltaAngle;
        }
        return projectiles;
    }

    /// <summary>
    /// Instantiates a projectile
    /// </summary>
    /// <param name="_scale">The size of the projectile</param>
    /// <param name="_speed">The speed of the projectile</param>
    /// <param name="_lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="_locationObject">The GameObject that will be used as the origin point of spawned projectiles. Null value will default to "Parent" position</param>
    /// <param name="_spawnOffset">The offset from the spawn point, Default is (0,0)</param>
    /// <param name="_directionAngle">The angle to move it towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    private GameObject CreateProjectile(float _scale, float _speed, float _lifetime, GameObject _locationObject, Vector2 _spawnOffset, float _directionAngle = 0.0f)
    {
        GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab);
        if (_locationObject != null)
        {
            projectile.transform.position = _locationObject.transform.position;
        }
        else
        {
            projectile.transform.position = parent.transform.position;
        }
        projectile.transform.position += (Vector3)_spawnOffset;
        projectile.gameObject.transform.rotation = Quaternion.Euler(0, 180.0f, -_directionAngle);
        projectile.gameObject.transform.localScale = Vector3.one * _scale;
        ProjectileBase projScript = projectile.GetComponent<ProjectileBase>();
        if (projScript != null)
        {
            projScript.SetDirection(new Vector3((float)Math.Cos(AngleMath.ToRadians(_directionAngle)), (float)Math.Sin(AngleMath.ToRadians(_directionAngle)), 0));
            projScript.SetSpeed(_speed);
            projScript.SetLifetime(_lifetime);
            projScript.SetParent(parent);
        }
        return projectile;
    }
    #endregion
}