using System;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectiles : NPCBaseAction
{
    GameObject projectilePrefab;

    public FireProjectiles(GameObject _projectilePrefab, Transform _parent) : this(_projectilePrefab, _parent, 1.0f) {}
    public FireProjectiles(GameObject _projectilePrefab, Transform _parent, float _actionDuration) : base(_parent, _actionDuration) { projectilePrefab = _projectilePrefab; }

    #region Helper Functions
    /// <summary>
    /// Converts an angle in Degrees to Radians
    /// </summary>
    /// <param name="degrees">The angle to convert</param>
    /// <returns>The angle in radians bounded to (0,2pi) </returns>
    private float ToRadians(float degrees)
    {
        return (degrees * ((float)Math.PI / 180.0f));
    }

    /// <summary>
    /// Gets the angle of a vector
    /// </summary>
    /// <param name="direction">The vector to get the angle of</param>
    /// <returns>The angle of the vector in degrees</returns>
    private float GetVectorDirection(Vector2 direction)
    {
        float angle = Vector2.Angle(Vector2.right, direction.normalized);
        Vector3 cross = Vector3.Cross(Vector2.right, direction.normalized);
        if (cross.z < 0)
        {
            angle = 360 - angle;
        }
        return angle;
    }
    #endregion

    #region Emit Projectile
    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(ProjectileSettings settings, float fireAngle = 0.0f)
    {
        return CreateProjectile(settings.Scale, settings.Speed, settings.Lifetime, fireAngle);
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="direction">The direction to fire towards</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(ProjectileSettings settings, Vector2 direction)
    {
        return CreateProjectile(settings.Scale, settings.Speed, settings.Lifetime, direction);
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
        return EmitCircle(settings.Count, settings.Scale, settings.Speed, settings.Lifetime, fireAngleOffset);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="directionOffset">The direction to offset the start direction by</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(ProjectileSettings settings, Vector2 directionOffset)
    {
        return EmitCircle(settings.Count, settings.Scale, settings.Speed, settings.Lifetime, GetVectorDirection(directionOffset));
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
    public List<GameObject> EmitCircle(int count, float scale, float speed, float lifetime, float fireAngleOffset = 0.0f)
    {
        List<GameObject> projectiles = new List<GameObject>();
        if (count <= 1)
        {
            projectiles.Add(CreateProjectile(scale, speed, lifetime));
            return projectiles;
        }

        float deltaAngle = 360.0f / (float)count;
        float currentAngle = fireAngleOffset;

        for (int i = 1; i <= count; i++)
        {
            projectiles.Add(CreateProjectile(scale, speed, lifetime, currentAngle));
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
    public List<GameObject> EmitCircle(int count, float scale, float speed, float lifetime, Vector2 directionOffset)
    {
        return EmitCircle(count, scale, speed, lifetime, GetVectorDirection(directionOffset));
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
        return EmitCircle(4, settings.Scale, settings.Speed, settings.Lifetime, fireAngleOffset);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="settings">The projectile settings</param>
    /// <param name="directionOffset">The direction to offset the direction angle by</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(ProjectileSettings settings, Vector2 directionOffset)
    {
        return EmitCircle(4, settings.Scale, settings.Speed, settings.Lifetime, directionOffset);
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
        return CreateProjectileArc(new List<GameObject>(), arcAngle, settings.Count, settings.Scale, settings.Speed, settings.Lifetime, fireAngle);
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
        return CreateProjectileArc(new List<GameObject>(), arcAngle, settings.Count, settings.Scale, settings.Speed, settings.Lifetime, direction);
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
    public List<GameObject> EmitArc(float arcAngle, int count, float scale, float speed, float lifetime, float fireAngle = 0.0f)
    {
        return CreateProjectileArc(new List<GameObject>(), arcAngle, count, scale, speed, lifetime, fireAngle);
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
    public List<GameObject> EmitArc(float arcAngle, int count, float scale, float speed, float lifetime, Vector2 direction)
    {
        return CreateProjectileArc(new List<GameObject>(), arcAngle, count, scale, speed, lifetime, direction);
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
    private List<GameObject> CreateProjectileArc(List<GameObject> projectileList, float arcAngle, int count, float scale, float speed, float lifetime, float fireAngle = 0.0f)
    {
        if (count <= 1)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, fireAngle));
            return projectileList;
        }
        float deltaAngle = arcAngle / (count - 1.0f);
        float startAngle = fireAngle + (-arcAngle / 2.0f);
        float currentAngle = startAngle;
        for (int i = 1; i <= count; i++)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, currentAngle));
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
    private List<GameObject> CreateProjectileArc(List<GameObject> projectileList, float arcAngle, int count, float scale, float speed, float lifetime, Vector2 direction)
    {
        if (count <= 1)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, direction));
            return projectileList;
        }
        return CreateProjectileArc(projectileList, arcAngle, count, scale, speed, lifetime, GetVectorDirection(direction));
    }

    /// <summary>
    /// Instantiates a projectile
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="directionAngle">The angle to move it towards in degrees</param>
    /// <returns>The instantiated projectile</returns>
    private GameObject CreateProjectile(float scale, float speed, float lifetime, float directionAngle = 0.0f)
    {
        GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, parent);
        projectile.gameObject.transform.localScale = Vector3.one * scale;
        ProjectileBase projScript = projectile.GetComponent<ProjectileBase>();
        if (projScript != null)
        {
            projScript.setDirection(new Vector3((float)Math.Cos(ToRadians(directionAngle)), (float)Math.Sin(ToRadians(directionAngle)), 0));
            projScript.setSpeed(speed);
            projScript.setLifetime(lifetime);
        }
        return projectile;
    }

    /// <summary>
    /// Instantiates a projectile
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <param name="direction">The direction to move it in</param>
    /// <returns>The instantiated projectile</returns>
    private GameObject CreateProjectile(float scale, float speed, float lifetime, Vector2 direction)
    {
        return CreateProjectile(scale, speed, lifetime, GetVectorDirection(direction));
    }
    #endregion
}