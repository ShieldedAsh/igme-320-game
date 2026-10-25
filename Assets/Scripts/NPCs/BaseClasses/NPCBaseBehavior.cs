using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class NPCBaseBehavior : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected List<GameObject> projectilePrefabs;
    [SerializeField]
    protected List<GameObject> meleeHitboxPrefabs;
    [SerializeField, Min(1)]
    protected int maxHealth;
    [SerializeField]
    protected GameObject player;

    [SerializeField]
    protected int currentHealth;
    protected List<NPCBaseAction> actions;
    protected NPCBaseAction currentAction;
    protected IEnumerator? currentCoroutine;
    protected float timeTillAction;
    protected int phaseEvents;
    protected int currentPhaseEvent;
    protected List<GameObject> projectiles;

    protected SpriteRenderer sr;
    protected Color baseColor;
    protected List<ProjectileSettings> projectileSettings;
    protected List<MeleeSettings> meleeSettings;

    /// <summary>
    /// The max health of the NPC
    /// </summary>
    public int MaxHealth { get => maxHealth; }

    /// <summary>
    /// The NPC's current health
    /// </summary>
    public int Health { get => currentHealth; }

    // Set as virtual in case child needs to add to Start()
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        timeTillAction = 0;
        if (actions == null)
        {
            actions = new List<NPCBaseAction>();
        }
        if (projectilePrefabs == null)
        {
            projectilePrefabs = new List<GameObject>();
        }
        if (projectiles == null)
        {
            projectiles = new List<GameObject>();
        }
        if (player == null)
        {
            if (FindFirstObjectByType<PlayerMovement>() != null)
            {
                player = FindFirstObjectByType<PlayerMovement>().gameObject;
            }
        }
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        if (projectileSettings == null)
        {
            projectileSettings = new List<ProjectileSettings>();
        }
        if (meleeSettings == null)
        {
            meleeSettings = new List<MeleeSettings>();
        }
        currentCoroutine = null;
    }

    // If there is more we need to do per update, put here
    // Take action should be specifically for "actions"
    void FixedUpdate()
    {
        timeTillAction -= Time.fixedDeltaTime;
        if (sr.color != baseColor)
        {
            sr.color = baseColor;
        }
        if (CanAct())
        {
            timeTillAction = Act();
            //update phase counter
            if (currentPhaseEvent >= phaseEvents)
            {
                currentPhaseEvent = 0;
            }
            else
            {
                currentPhaseEvent++;
            }
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
        if (animator != null)
        {
            try
            {
                animator.SetTrigger("isDamaged");
            }
            catch
            {
                sr.color = new Color(1f, 0.9f, 0.9f, 1f);
            }
        }
        else
        {
            sr.color = new Color(1f, 0.9f, 0.9f, 1f);
        }
        currentHealth -= damage;
    }

    protected bool CanAct()
    {
        if (actions != null && timeTillAction <= 0 && actions.Count > 0)
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
            projectiles.Remove(projectile);
        }
    }

    protected T ConvertActionTo<T>(NPCBaseAction action)
    {
        if (action is T && typeof(NPCBaseAction).IsAssignableFrom(typeof(T)))
        {
            return (T)Convert.ChangeType(action, typeof(T));
        }
        throw new TypeAccessException();
    }

    protected void TurnLeft()
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    }

    protected void TurnRight()
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
    }

    protected void TurnTowards(Vector2 loc)
    {
        if (loc.x > this.gameObject.transform.position.x)
        {
            TurnRight();
        }
        else
        {
            TurnLeft();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerAttack pa;
        if (collision.tag == "WeaponHitBox" && (pa = collision.gameObject.transform.parent.GetComponent<PlayerAttack>()) != null)
        {
            TakeDamage(pa.damage);
            //Check to see if there is a specific way this should be destroyed
            Destroy(collision.gameObject);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            InteruptCurrentEvent();
            
        }
    }

    public void IncrementEventCounter()
    {
        currentPhaseEvent++;
        if (currentPhaseEvent >= phaseEvents)
        {
            currentPhaseEvent = 0;
        }
    }

    public void IncrementEventCounterAndWait(float waitFor = 0.0f)
    {
        IncrementEventCounter();
        timeTillAction = waitFor;
    }

    protected void InteruptCurrentEvent(float pauseTime = 0.0f, bool overrideActionCooldown = false)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        IncrementEventCounter();
        if (timeTillAction > pauseTime || overrideActionCooldown)
        {
            timeTillAction = pauseTime;
        }
    }

    public void OverwriteActionCooldown(float cooldown)
    {
        timeTillAction = cooldown;
    }
}
