using System;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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

    public List<GameObject> EmitCircle(int count, float scale, float speed, float lifetime)
    {
        List<GameObject> projectiles = new List<GameObject>();
        float deltaT = (float)((2.0f * Math.PI) / (float)count);
        float startAngle = 0;
        CreateProjectiles(projectiles, startAngle, deltaT, count, scale, speed, lifetime);
        return projectiles;
    }

    public List<GameObject> EmitTriangle(float angleRange, int count, Vector2 direction, float scale, float speed, float lifetime)
    {
        List<GameObject> projectiles = new List<GameObject>();
        float radianRange = angleRange * ((float)Math.PI / 180.0f);
        float fireAngle = Vector2.Angle(Vector2.right, direction) * ((float)Math.PI / 180.0f);

        float deltaT;
        float startAngle;
        if (count <= 1)
        {
            deltaT = 0;
            startAngle = fireAngle;
        }
        else
        {
            deltaT = radianRange / (count - 1.0f);
            startAngle = (fireAngle + (-radianRange / 2.0f)) % (float)(Math.PI * 2);
        }
        CreateProjectiles(projectiles, startAngle, deltaT, count, scale, speed, lifetime);
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

    private void CreateProjectiles(List<GameObject> projectileList, float startAngle, float deltaT, int count, float scale, float speed, float lifetime)
    {
        float theta = startAngle;
        for (int i = 1; i <= count; i++)
        {
            GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, parent);
            projectile.gameObject.transform.localScale = Vector3.one * scale;
            ProjectileBase projScript = projectile.GetComponent<ProjectileBase>();
            if (projScript != null)
            {
                projScript.setDirection(new Vector3((float)Math.Cos(theta), (float)Math.Sin(theta), 0));
                projScript.setSpeed(speed);
                projScript.setLifetime(lifetime);
            }
            theta += deltaT;
            theta = theta % (float)(Math.PI * 2);
            projectileList.Add(projectile);
        }
    }
}
