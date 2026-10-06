using UnityEngine;
using System.Collections.Generic;

public class ProjectileEmitterBehavior : NPCBaseBehavior
{
    [SerializeField, Range(1,180)]
    float angleRange;
    [SerializeField]
    GameObject target;
    [SerializeField, Min(1)]
    int projectileCount;

    protected override void Start()
    {
        base.Start();
        actCooldown = 0;
        if (projectilePrefabs.Count > 0)
        {
            attacks.Add(new FireProjectiles(projectilePrefabs[0], this.transform, 2f));
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
                    //List<GameObject> newProjectiles = ((FireProjectiles)attacks[0]).EmitCircle(projectileCount, 0.5f, 2.0f, 6.0f);
                    List<GameObject> newProjectiles = ((FireProjectiles)attacks[0]).EmitTriangle(angleRange, projectileCount, (Vector2)target.transform.position.normalized, 0.5f, 2.0f, 6.0f);
                    spawnedProjectiles.AddRange(newProjectiles);
                }
                actCooldown = attacks[0].Duration;
            }
        }
        actCooldown -= Time.fixedDeltaTime;
    }
}
