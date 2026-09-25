using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseBehavior : MonoBehaviour
{
    [SerializeField, Min(1)]
    protected int maxHealth;
    protected int currentHealth;

    // The list of actions the NPC can take
    [SerializeReference]
    protected List<Action> actions;

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
    }

    // If there is more we need to do per update, put here
    // Take action should be specifically for "actions"
    void FixedUpdate()
    {
        ChooseAction();
    }

    // All NPC's should have some sort of collision behavior
    protected abstract void OnCollisionEnter2D(Collision2D collision);

    /// <summary>
    /// The logic for how NPC's use the actions in their action list
    /// </summary>
    protected abstract void ChooseAction();

    /// <summary>
    /// Reduces the NPC's health by damage taken
    /// </summary>
    /// <param name="damage">The amount of health to reduce by</param>
    protected virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
