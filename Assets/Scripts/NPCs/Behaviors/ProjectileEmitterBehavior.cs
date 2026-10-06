using UnityEngine;
using System.Collections.Generic;

public class ProjectileEmitterBehavior : NPCBaseBehavior
{
    [SerializeField, Range(1,180)]
    float angleRange;
    [SerializeField]
    GameObject target;

    protected override void Start()
    {
        base.Start();
        actCooldown = 0;
        if (projectilePrefabs.Count > 0)
        {
            attacks.Add(new FireProjectiles(projectilePrefabs[0], this.transform, 0.3f));
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
                    List<GameObject> newProjectiles = ((FireProjectiles)attacks[0]).EmitTriangle(angleRange, 5, (Vector2)target.transform.position.normalized, 0.5f, 5.0f, 1.0f);
                    spawnedProjectiles.AddRange(newProjectiles);
                }
                actCooldown = attacks[0].Duration;
            }
        }
        actCooldown -= Time.fixedDeltaTime;
    }
}
