using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class NPCBaseBehavior : MonoBehaviour
{

    [SerializeField, Min(1)]
    protected int maxHealth;
    protected int currentHealth;

    protected List<NPCBaseAttack> attacks;
    [SerializeField]
    protected NPCBaseAttack current;
    [SerializeField]
    protected float actCooldown;
    [SerializeField]
    protected List<GameObject> projectilePrefabs;
    [SerializeField]
    protected List<GameObject> spawnedProjectiles;

    [SerializeField] 
    protected Animator animator;

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
        if (attacks == null)
        {
            attacks = new List<NPCBaseAttack>();
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
        Act();
        for (int i = 0; i < spawnedProjectiles.Count; i++)
        {
            if (spawnedProjectiles[i] == null)
            {
                spawnedProjectiles.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// The logic for how NPC's act
    /// </summary>
    protected abstract void Act();

    /// <summary>
    /// Reduces the NPC's health by damage taken
    /// </summary>
    /// <param name="damage">The amount of health to reduce by</param>
    protected virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    /// <summary>
    /// Moves the NPC to the destination over time
    /// </summary>
    /// <param name="transform">The transform of the NPC</param>
    /// <param name="destination">The destination to move to</param>
    /// <param name="deltaTime">The time since the last frame</param>
    /// <param name="moveTime">The time to move between points</param>
    /// <returns>A midpoint between the current location and the destination</returns>
    protected Vector2 MoveTo(Transform transform, Vector2 destination, float deltaTime, float moveTime = 1)
    {
        return Vector2.Lerp(transform.position, destination, deltaTime/moveTime);
    }
}
