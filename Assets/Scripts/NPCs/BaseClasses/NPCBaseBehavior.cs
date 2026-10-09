using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class NPCBaseBehavior : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected List<GameObject> projectilePrefabs;
    [SerializeField, Min(1)]
    protected int maxHealth;

    protected int currentHealth;
    protected List<NPCBaseAction> actions;
    protected NPCBaseAction currentAction;
    protected float timeTillAction;
    protected List<GameObject> spawnedProjectiles;

    

    /// <summary>
    /// The max health of the NPC
    /// </summary>
    public int MaxHealth { get; }

    /// <summary>
    /// The NPC's current health
    /// </summary>
    public int Health { get => currentHealth; }

    // Set as virtual in case child needs to add to Start()
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        if (actions == null)
        {
            actions = new List<NPCBaseAction>();
        }
        if (projectilePrefabs == null)
        {
            projectilePrefabs = new List<GameObject>();
        }
        if (spawnedProjectiles == null)
        {
            spawnedProjectiles = new List<GameObject>();
        }
    }

    // If there is more we need to do per update, put here
    // Take action should be specifically for "actions"
    void FixedUpdate()
    {
        timeTillAction -= Time.fixedDeltaTime;
        if (CanAct())
        {
            timeTillAction = Act();
        }
    }

    /// <summary>
    /// The logic for how NPCs act
    /// </summary>
    /// <returns>The amount of time until the next action</returns>
    protected abstract float Act();

    /// <summary>
    /// Reduces the NPC's health by damage taken
    /// </summary>
    /// <param name="damage">The amount of health to reduce by</param>
    protected virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    protected bool CanAct()
    {
        if (timeTillAction <= 0 && actions.Count > 0)
        {
            return true;
        }
        return false;
    }

    // Projectiles should call this upon death to remove themselves from the list
    public void RemoveProjectile(GameObject projectile)
    {
        if (projectile.GetComponent<ProjectileBase>() != null)
        {
            spawnedProjectiles.Remove(projectile);
        }
    }
}
