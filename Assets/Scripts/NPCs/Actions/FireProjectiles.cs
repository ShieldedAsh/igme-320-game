using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * To Do:
 * Emit X-Sequence (spawn delay between projectiles)
 */

public class FireProjectiles : NPCBaseAction
{
    GameObject projectilePrefab;

    public GameObject ProjectilePrefab { get => projectilePrefab; }

    public FireProjectiles(GameObject _projectilePrefab, Transform _parent, float _actionDuration = 1.0f) : base(_parent, _actionDuration) { projectilePrefab = _projectilePrefab; }

    public void SetProjectile(GameObject projectile)
    {
        projectilePrefab = projectile;
    }

    #region Emit Projectile
    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(ProjectileSettings settings, float fireAngle = 0.0f)
    {
        return CreateProjectile(settings.Scale, settings.Speed, settings.Lifetime, fireAngle, settings.SpawnLocation);
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="direction">The direction to fire towards</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(ProjectileSettings settings, Vector2 direction)
    {
        return CreateProjectile(settings.Scale, settings.Speed, settings.Lifetime, direction, settings.SpawnLocation);
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(float scale, float speed, float lifetime, float fireAngle = 0.0f)
    {
        return CreateProjectile(scale, speed, lifetime, fireAngle);
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// /// <param name="direction">The direction to fire towards</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(float scale, float speed, float lifetime, Vector2 direction)
    {
        return CreateProjectile(scale, speed, lifetime, direction);
    }
    #endregion

    #region Emit Circle
    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject>EmitCircle(ProjectileSettings settings, float fireAngleOffset = 0.0f)
    {
        return EmitCircle(settings.Count, settings.Scale, settings.Speed, settings.Lifetime, fireAngleOffset, settings.SpawnLocation);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="directionOffset">The direction to offset the start direction by</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(ProjectileSettings settings, Vector2 directionOffset, Transform spawnLocation = null)
    {
        return EmitCircle(settings.Count, settings.Scale, settings.Speed, settings.Lifetime, AngleMath.GetVectorDirection(directionOffset), settings.SpawnLocation);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="count">The amount of projectiles to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(int count, float scale, float speed, float lifetime, float fireAngleOffset = 0.0f, Transform spawnLocation = null)
    {
        List<GameObject> projectiles = new List<GameObject>();
        if (count <= 1)
        {
            projectiles.Add(CreateProjectile(scale, speed, lifetime, spawnLocation));
            return projectiles;
        }

        float deltaAngle = 360.0f / (float)count;
        float currentAngle = fireAngleOffset;

        for (int i = 1; i <= count; i++)
        {
            projectiles.Add(CreateProjectile(scale, speed, lifetime, currentAngle, spawnLocation));
            currentAngle = currentAngle + deltaAngle;
        }
        return projectiles;
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="count">The amount of projectiles to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="directionOffset">The direction to offset the start direction by</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(int count, float scale, float speed, float lifetime, Vector2 directionOffset, Transform spawnLocation = null)
    {
        return EmitCircle(count, scale, speed, lifetime, AngleMath.GetVectorDirection(directionOffset));
    }
    #endregion

    #region Emit Cross
    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(ProjectileSettings settings, float fireAngleOffset = 0.0f)
    {
        return EmitCircle(4, settings.Scale, settings.Speed, settings.Lifetime, fireAngleOffset, settings.SpawnLocation);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="directionOffset">The direction to offset the direction angle by</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(ProjectileSettings settings, Vector2 directionOffset)
    {
        return EmitCircle(4, settings.Scale, settings.Speed, settings.Lifetime, directionOffset, settings.SpawnLocation);
    }


    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(float scale, float speed, float lifetime, float fireAngleOffset = 0.0f)
    {
        return EmitCircle(4, scale, speed, lifetime, fireAngleOffset);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="directionOffset">The direction to offset the direction angle by</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(float scale, float speed, float lifetime, Vector2 directionOffset)
    {
        return EmitCircle(4, scale, speed, lifetime, directionOffset);
    }
    #endregion

    #region EmitArc
    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="settings">The projectile settings</param>
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float arcAngle, ProjectileSettings settings, float fireAngle = 0.0f)
    {
        return CreateProjectileArc(new List<GameObject>(), arcAngle, settings.Count, settings.Scale, settings.Speed, settings.Lifetime, fireAngle, settings.SpawnLocation);
    }

    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="settings">The projectile settings</param>
    /// <param name="direction">The direction to fire towards</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float arcAngle, ProjectileSettings settings, Vector2 direction)
    {
        return CreateProjectileArc(new List<GameObject>(), arcAngle, settings.Count, settings.Scale, settings.Speed, settings.Lifetime, direction, settings.SpawnLocation);
    }

    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="count">The amount of projectiles to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float arcAngle, int count, float scale, float speed, float lifetime, float fireAngle = 0.0f, Transform spawnLocation = null)
    {
        return CreateProjectileArc(new List<GameObject>(), arcAngle, count, scale, speed, lifetime, fireAngle, spawnLocation);
    }

    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="count">The amount of projectiles to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="direction">The direction to fire towards</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float arcAngle, int count, float scale, float speed, float lifetime, Vector2 direction, Transform spawnLocation = null)
    {
        return CreateProjectileArc(new List<GameObject>(), arcAngle, count, scale, speed, lifetime, direction, spawnLocation);
    }
    #endregion

    #region Projectile Creation
    /// <summary>
    /// Instantiates projectiles in an arc and adds them to projectileList
    /// </summary>
    /// <param name="projectileList">The list to add to</param>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="count">The amount of projectiles to instantiate</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <returns>A list of instantiated projectiles</returns>
    private List<GameObject> CreateProjectileArc(List<GameObject> projectileList, float arcAngle, int count, float scale, float speed, float lifetime, float fireAngle = 0.0f, Transform spawnLocation = null)
    {
        if (count <= 1)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, fireAngle, spawnLocation));
            return projectileList;
        }
        float deltaAngle = arcAngle / (count - 1.0f);
        float startAngle = fireAngle + (-arcAngle / 2.0f);
        float currentAngle = startAngle;
        for (int i = 1; i <= count; i++)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, currentAngle, spawnLocation));
            currentAngle += deltaAngle;
        }
        return projectileList;
    }

    /// <summary>
    /// Instantiates projectiles in an arc and adds them to projectileList
    /// </summary>
    /// <param name="projectileList">The list to add to</param>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="count">The amount of projectiles to instantiate</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="direction">The direction to fire towards</param>
    /// <returns>A list of instantiated projectiles</returns>
    private List<GameObject> CreateProjectileArc(List<GameObject> projectileList, float arcAngle, int count, float scale, float speed, float lifetime, Vector2 direction, Transform spawnLocation = null)
    {
        if (count <= 1)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, direction, spawnLocation));
            return projectileList;
        }
        return CreateProjectileArc(projectileList, arcAngle, count, scale, speed, lifetime, AngleMath.GetVectorDirection(direction), spawnLocation);
    }

    /// <summary>
    /// Instantiates a projectile
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="directionAngle">The angle to move it towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    private GameObject CreateProjectile(float scale, float speed, float lifetime, float directionAngle = 0.0f, Transform spawnLocation = null)
    {
        GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab);
        if (spawnLocation != null)
        {
            projectile.gameObject.transform.position = spawnLocation.position;
        }
        projectile.gameObject.transform.rotation = Quaternion.Euler(0, 180.0f, -directionAngle);
        projectile.gameObject.transform.localScale = Vector3.one * scale;
        ProjectileBase projScript = projectile.GetComponent<ProjectileBase>();
        if (projScript != null)
        {
            projScript.SetDirection(new Vector3((float)Math.Cos(AngleMath.ToRadians(directionAngle)), (float)Math.Sin(AngleMath.ToRadians(directionAngle)), 0));
            projScript.SetSpeed(speed);
            projScript.SetLifetime(lifetime);
            projScript.SetParent(parent);
        }
        return projectile;
    }

    private GameObject CreateProjectile(float scale, float speed, float lifetime, Transform spawnLocation = null)
    {
        return CreateProjectile(scale, speed, lifetime, 0.0f, spawnLocation);
    }

    /// <summary>
    /// Instantiates a projectile
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="direction">The direction to move it in</param>
    /// <returns>The instantiated projectile</returns>
    private GameObject CreateProjectile(float scale, float speed, float lifetime, Vector2 direction, Transform spawnLocation = null)
    {
        return CreateProjectile(scale, speed, lifetime, AngleMath.GetVectorDirection(direction), spawnLocation);
    }
    #endregion
}