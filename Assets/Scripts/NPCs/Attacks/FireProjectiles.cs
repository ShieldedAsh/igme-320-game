using System;
using UnityEngine;
using System.Collections.Generic;

public class FireProjectiles : NPCBaseAttack
{
    GameObject projectilePrefab;

    public FireProjectiles(GameObject _projectilePrefab, Transform _parent) : this(_projectilePrefab, _parent, 1.0f)
    {

    }

    public FireProjectiles(GameObject _projectilePrefab, Transform _parent, float _actionDuration) : base(_parent, _actionDuration)
    {
        projectilePrefab = _projectilePrefab;
    }

    public List<GameObject> EmitCircle(int projectileCount, float projectileScale, float projectileSpeed, float projectileLifetime)
    {
        List<GameObject> projectiles = new List<GameObject>();
        float deltaT = (float)((2.0f * Math.PI) / (float)projectileCount);
        float theta = 0;
        for (int i = 1; i <= projectileCount; i++)
        {
            GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, parent);
            projectile.gameObject.transform.localScale = Vector3.one * projectileScale;
            ProjectileBase projScript = projectile.GetComponent<ProjectileBase>();
            if (projScript != null)
            {
                projScript.setDirection(new Vector3((float)Math.Cos(theta), (float)Math.Sin(theta), 0));
                projScript.setSpeed(projectileSpeed);
                projScript.setLifetime(projectileLifetime);
            }
            theta += deltaT;
            projectiles.Add(projectile);
        }
        return projectiles;
    }

    public List<GameObject> EmitTriangle(float angleRange, int projectileCount, Vector2 direction, float projectileScale, float projectileSpeed, float projectileLifetime)
    {
        List<GameObject> projectiles = new List<GameObject>();
        float deltaT = (angleRange * ((float)Math.PI/180.0f)) / (float)projectileCount;
        float angleMod = 0;

        if ((direction.normalized.x > 0 && direction.normalized.y > 0) || (direction.normalized.x < 0 && direction.normalized.y > 0))
        {
            angleMod = (float)Math.Acos(direction.normalized.x);
            Debug.Log("Angle Mod: " + angleMod);
        }
        else if ((direction.normalized.x < 0 && direction.normalized.y < 0) || (direction.normalized.x > 0 && direction.normalized.y < 0))
        {

            angleMod = ((float)Math.PI * 2.0f) - (float)Math.Acos(direction.normalized.x);
            Debug.Log("Angle Mod: " + angleMod);
        }
        
        float theta = angleMod;
        for (int i = 1; i <= projectileCount; i++)
        {
            GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, parent);
            projectile.gameObject.transform.localScale = Vector3.one * projectileScale;
            ProjectileBase projScript = projectile.GetComponent<ProjectileBase>();
            if (projScript != null)
            {
                projScript.setDirection(new Vector3((float)Math.Cos(theta), (float)Math.Sin(theta), 0));
                projScript.setSpeed(projectileSpeed);
                projScript.setLifetime(projectileLifetime);
            }
            theta += deltaT;
            projectiles.Add(projectile);
        }
        return projectiles;
    }

    public List<GameObject> EmitLine()
    {
        return null;
    }

    public List<GameObject> EmitCross()
    {
        return null;
    }
}
