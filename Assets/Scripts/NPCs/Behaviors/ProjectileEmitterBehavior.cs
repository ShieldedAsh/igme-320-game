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
    ProjectileSettings settings;

    protected override void Start()
    {
        base.Start();
        timeTillAction = 0;
        if (actionTime < 0.1f)
        {
            actionTime = 0.1f;
        }
        actions.Add(new FireProjectiles(projectilePrefabs[0], this.transform, actionTime));
        currentAction = actions[0];
        directionAngle = 0.0f;
        arcAngle = 90.0f;
        settings = new ProjectileSettings(8, 0.5f, 2.0f, 0.5f);
    }

    protected override float Act()
    {
        currentAction.SetActionDuration(actionTime);
        List<GameObject> newProjectiles = new List<GameObject>();
        Vector2 direction = (Vector2)target.transform.position.normalized;
        Randomize();
        try
        {
            FireProjectiles fp = ConvertActionTo<FireProjectiles>(currentAction);
            switch (Random.Range(0, 8))
            {
                case 0:
                    Debug.Log("Direction Projectile");
                    projectiles.Add(fp.EmitProjectile(settings, direction));
                    break;
                case 1:
                    Debug.Log($"{directionAngle} degree Projectile");
                    projectiles.Add(fp.EmitProjectile(settings, directionAngle));
                    break;
                case 2:
                    Debug.Log("Direction Circle - " + settings.Count);
                    newProjectiles = fp.EmitCircle(settings, direction);
                    break;
                case 3:
                    Debug.Log($"{directionAngle} degree Circle - " + settings.Count);
                    newProjectiles = fp.EmitCircle(settings, directionAngle);
                    break;
                case 4:
                    Debug.Log("Direction Cross");
                    newProjectiles = fp.EmitCross(settings, direction);
                    break;
                case 5:
                    Debug.Log($"{directionAngle} degree Cross");
                    newProjectiles = fp.EmitCross(settings, directionAngle);
                    break;
                case 6:
                    Debug.Log("Direction Arc - " + settings.Count);
                    newProjectiles = fp.EmitArc(arcAngle, settings, direction);
                    break;
                case 7:
                    Debug.Log($"{directionAngle} degree Arc - " + settings.Count);
                    newProjectiles = fp.EmitArc(arcAngle, settings, directionAngle);
                    break;
            }
        }
        catch (System.TypeAccessException e)
        {
            Debug.Log(e);
        }

        if (newProjectiles.Count > 0)
        {
            projectiles.AddRange(newProjectiles);
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
        Debug.Log($"{settings.Count} {settings.Scale} {settings.Speed} {settings.Lifetime}");
        directionAngle = Random.Range(0.0f, 359.9f);
        arcAngle = Random.Range(1.0f, 180.0f);
        settings.SetCount(Random.Range(1, 10));
        settings.SetScale(Random.Range(0.25f, 1.0f));
        settings.SetSpeed(Random.Range(2.0f / actionTime, 8.0f / actionTime));
        settings.SetLifetime(Random.Range(actionTime / 2.0f, actionTime * 2.0f));
    }
}
