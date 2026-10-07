using UnityEngine;
using System.Collections.Generic;

public class ProjectileEmitterBehavior : NPCBaseBehavior
{
    [SerializeField, Range(0,360)]
    float angleRange;
    [SerializeField]
    GameObject target;
    //float angle;
    [SerializeField, Min(1)]
    int projectileCount;

    protected override void Start()
    {
        base.Start();
        actCooldown = 0;
        if (projectilePrefabs.Count > 0)
        {
            attacks.Add(new FireProjectiles(projectilePrefabs[0], this.transform, 1.0f));
        }
    }

    protected override void Act()
    {
        if (actCooldown <= 0)
        {
            if (attacks.Count > 0)
            {
                if (attacks[0] is FireProjectiles)
                {
                    //List<GameObject> newProjectiles = ((FireProjectiles)attacks[0]).EmitCircle(projectileCount, 0.5f, 5.0f, 2.0f);
                    List<GameObject> newProjectiles = ((FireProjectiles)attacks[0]).EmitArc((Vector2)target.transform.position.normalized, angleRange, projectileCount, 0.5f, 5.0f, 1.0f);
                    spawnedProjectiles.AddRange(newProjectiles);
                }
                actCooldown = attacks[0].Duration;
            }
        }
        actCooldown -= Time.fixedDeltaTime;
    }
}
