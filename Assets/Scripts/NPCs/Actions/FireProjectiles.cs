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
    /// <returns>The angle of the vector</returns>
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
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>The instantiated projectile</returns>
    public GameObject EmitProjectile(float scale, float speed, float lifetime, float fireAngle)
    {
        return CreateProjectile(scale, speed, lifetime, fireAngle);
    }

    /// <summary>
    /// Fires one projectile
    /// </summary>
    /// <param name="direction">The direction to fire towards</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
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
    /// <param name="count">The amount of projectile to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(int count, float scale, float speed, float lifetime)
    {
        return EmitCircle(count, scale, speed, lifetime, 0.0f);
    }

    /// <summary>
    /// Fires projectiles in a circle
    /// </summary>
    /// <param name="fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <param name="count">The amount of projectile to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(int count, float scale, float speed, float lifetime, float fireAngleOffset)
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
    /// <param name="directionOffset">The direction to offset the start direction by</param>
    /// <param name="count">The amount of projectile to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCircle(int count, float scale, float speed, float lifetime, Vector2 directionOffset)
    {
        float angleOffset = GetVectorDirection(directionOffset);
        return EmitCircle(count, scale, speed, lifetime, angleOffset);
    }
    #endregion

    #region Emit Cross
    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(float scale, float speed, float lifetime)
    {
        return EmitCircle(4, scale, speed, lifetime);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="fireAngleOffset">The angle to offset the start angle by in degrees</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitCross(float scale, float speed, float lifetime, float fireAngleOffset)
    {
        return EmitCircle(4, scale, speed, lifetime, fireAngleOffset);
    }

    /// <summary>
    /// Fires four projectiles in a cross
    /// </summary>
    /// <param name="directionOffset">The direction to offset the direction angle by</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
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
    /// <param name="arcAngle">How wide the arc is in degrees</param>
    /// <param name="count">The amount of projectile to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float arcAngle, int count, float scale, float speed, float lifetime)
    {
        List<GameObject> projectiles = new List<GameObject>();
        CreateProjectileArc(projectiles, arcAngle, count, scale, speed, lifetime);
        return projectiles;
    }

    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <param name="arcAngle">How wide the arc is in degrees</param>
    /// <param name="count">The amount of projectile to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float arcAngle, int count, float scale, float speed, float lifetime, float fireAngle)
    {
        List<GameObject> projectiles = new List<GameObject>();
        CreateProjectileArc(projectiles, arcAngle, count, scale, speed, lifetime, fireAngle);
        return projectiles;
    }

    /// <summary>
    /// Fires projectiles in an arc
    /// </summary>
    /// <param name="direction">The direction to fire towards</param>
    /// <param name="arcAngle">How width of the arc is in degrees</param>
    /// <param name="count">The amount of projectile to fire</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>A list of the instantiated projectiles</returns>
    public List<GameObject> EmitArc(float arcAngle, int count, float scale, float speed, float lifetime, Vector2 direction)
    {
        List<GameObject> projectiles = new List<GameObject>();
        CreateProjectileArc(projectiles, arcAngle, count, scale, speed, lifetime, direction);
        return projectiles;
    }
    #endregion

    #region Projectile Creation
    /// <summary>
    /// Instantiates projectiles in an arc and adds them to projectileList
    /// </summary>
    /// <param name="projectileList">The list to add to</param>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="count">The amount of projectile to instantiate</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    private void CreateProjectileArc(List<GameObject> projectileList, float arcAngle, int count, float scale, float speed, float lifetime)
    {
        if (count <= 1)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime));
            return;
        }
        CreateProjectileArc(projectileList, arcAngle, count, scale, speed, lifetime, 0.0f);
    }

    /// <summary>
    /// Instantiates projectiles in an arc and adds them to projectileList
    /// </summary>
    /// <param name="projectileList">The list to add to</param>
    /// <param name="fireAngle">The angle to fire towards in degrees</param>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="count">The amount of projectile to instantiate</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    private void CreateProjectileArc(List<GameObject> projectileList, float arcAngle, int count, float scale, float speed, float lifetime, float fireAngle)
    {
        if (count <= 1)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, fireAngle));
            return;
        }
        float deltaAngle = arcAngle / (count - 1.0f);
        float startAngle = fireAngle + (-arcAngle / 2.0f);
        float currentAngle = startAngle;
        for (int i = 1; i <= count; i++)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, currentAngle));
            currentAngle += deltaAngle;
        }
    }

    /// <summary>
    /// Instantiates projectiles in an arc and adds them to projectileList
    /// </summary>
    /// <param name="projectileList">The list to add to</param>
    /// <param name="direction">The direction to fire towards</param>
    /// <param name="arcAngle">The width of the arc in degrees</param>
    /// <param name="count">The amount of projectile to instantiate</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    private void CreateProjectileArc(List<GameObject> projectileList, float arcAngle, int count, float scale, float speed, float lifetime, Vector2 direction)
    {
        if (count <= 1)
        {
            projectileList.Add(CreateProjectile(scale, speed, lifetime, direction));
            return;
        }
        float fireAngle = GetVectorDirection(direction);
        CreateProjectileArc(projectileList, arcAngle, count, scale, speed, lifetime, fireAngle);
    }

    /// <summary>
    /// Instantiates a projectile
    /// </summary>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>The instantiated projectile</returns>
    private GameObject CreateProjectile(float scale, float speed, float lifetime)
    {
        return CreateProjectile(scale, speed, lifetime, 0.0f);
    }

    /// <summary>
    /// Instantiates a projectile
    /// </summary>
    /// <param name="directionAngle">The angle to move it towards in degrees</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>The instantiated projectile</returns>
    private GameObject CreateProjectile(float scale, float speed, float lifetime, float directionAngle)
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
    /// <param name="direction">The direction to move it in</param>
    /// <param name="scale">The size of the projectile</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="lifetime">In seconds, how long until the projectile despawns</param>
    /// <returns>The instantiated projectile</returns>
    private GameObject CreateProjectile(float scale, float speed, float lifetime, Vector2 direction)
    {
        float fireAngle = GetVectorDirection(direction);
        return CreateProjectile(scale, speed, lifetime, fireAngle);
    }
    #endregion
}
