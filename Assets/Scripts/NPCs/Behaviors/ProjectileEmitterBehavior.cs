using UnityEngine;
using System.Collections.Generic;

public class ProjectileEmitterBehavior : NPCBaseBehavior
{
    [SerializeField]
    GameObject target;

    [SerializeField, Min(0.1f)]
    float actionTime;
    float directionAngle;
    
    float arcAngle;
    int projectileCount;
    float projectileScale;
    float projectileSpeed;
    float projectileLifetime;

    protected override void Start()
    {
        base.Start();
        timeTillAction = 0;
        if (actionTime < 0.1f)
        {
            actionTime = 0.1f;
        }
        if (projectilePrefabs.Count > 0)
        {
            actions.Add(new FireProjectiles(projectilePrefabs[0], this.transform, actionTime));
        }
        directionAngle = 0.0f;
        arcAngle = 90.0f;
        projectileCount = 8;
        projectileScale = 0.5f;
        projectileSpeed = 2.0f;
        projectileLifetime = 0.5f;
    }

    protected override float Act()
    {
        if (actions[0] is FireProjectiles)
        {
            actions[0].SetActionDuration(actionTime);
            currentAction = actions[0];
            List<GameObject> newProjectiles = new List<GameObject>();
            Vector2 direction = (Vector2)target.transform.position.normalized;
            Randomize();
            switch (Random.Range(0,8))
            {
                case 0:
                    Debug.Log("Direction Projectile");
                    spawnedProjectiles.Add(((FireProjectiles)currentAction).EmitProjectile(projectileScale, projectileSpeed, projectileLifetime, direction));
                    break;
                case 1:
                    Debug.Log($"{directionAngle} degree Projectile");
                    spawnedProjectiles.Add(((FireProjectiles)currentAction).EmitProjectile(projectileScale, projectileSpeed, projectileLifetime, directionAngle));
                    break;
                case 2:
                    Debug.Log("Direction Circle - " + projectileCount);
                    newProjectiles = ((FireProjectiles)currentAction).EmitCircle(projectileCount, projectileScale, projectileSpeed, projectileLifetime, direction);
                    break;
                case 3:
                    Debug.Log($"{directionAngle} degree Circle - " + projectileCount);
                    newProjectiles = ((FireProjectiles)currentAction).EmitCircle(projectileCount, projectileScale, projectileSpeed, projectileLifetime, directionAngle);
                    break;
                case 4:
                    Debug.Log("Direction Cross");
                    newProjectiles = ((FireProjectiles)currentAction).EmitCross(projectileScale, projectileSpeed, projectileLifetime, direction);
                    break;
                case 5:
                    Debug.Log($"{directionAngle} degree Cross");
                    newProjectiles = ((FireProjectiles)currentAction).EmitCross(projectileScale, projectileSpeed, projectileLifetime, directionAngle);
                    break;
                case 6:
                    Debug.Log("Direction Arc - " + projectileCount);
                    newProjectiles = ((FireProjectiles)currentAction).EmitArc(arcAngle, projectileCount, projectileScale, projectileSpeed, projectileLifetime, direction);
                    break;
                case 7:
                    Debug.Log($"{directionAngle} degree Arc - " + projectileCount );
                    newProjectiles = ((FireProjectiles)currentAction).EmitArc(arcAngle, projectileCount, projectileScale, projectileSpeed, projectileLifetime, directionAngle);
                    break;
            }
            if (spawnedProjectiles.Count > 0)
            {
                spawnedProjectiles.AddRange(newProjectiles);
            }
        }
        if (currentAction != null)
        {
            return currentAction.Duration;
        }
        else
        {
            return 1.0f;
        }
    }

    private void Randomize()
    {
        directionAngle = Random.Range(0.0f, 359.9f);
        arcAngle = Random.Range(1.0f, 180.0f);
        projectileCount = Random.Range(1, 10);
        projectileScale = Random.Range(0.25f, 1.0f);
        projectileSpeed = Random.Range(2.0f / actionTime, 8.0f / actionTime);
        projectileLifetime = Random.Range(actionTime / 2.0f, actionTime * 2.0f);
    }
}
