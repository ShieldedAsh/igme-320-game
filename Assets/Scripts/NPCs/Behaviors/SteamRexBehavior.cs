using System;
using UnityEngine;

enum SteamRexState
{
    Spawn,
    Phase1,
    Phase2,
    Phase3,
    Dead
}

public class SteamRexBehavior : NPCBaseBehavior
{
    SpriteRenderer sr;
    [SerializeField]
    GameObject player;
    SteamRexState state;
    int currentCycleIndex;
    bool hasUsedMelee;
    Vector2[] arenaMarks;
    Vector2 currentLocationTarget;
    ProjectileSettings projSettings;
    MeleeSettings meleeSettings;

    protected override void Start()
    {
        base.Start();
        state = SteamRexState.Spawn;
        hasUsedMelee = false;
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        actions.Add(new Move(this.transform, 2.0f));
        currentAction = actions[0];
        arenaMarks = new Vector2[3];
        arenaMarks[0] = new Vector2(-7.35f, -4.0f); //left
        arenaMarks[1] = new Vector2(7.35f, -4.0f); //right
        arenaMarks[2] = new Vector2(0.0f, -4.0f); //center floor
        currentLocationTarget = arenaMarks[0];
        if (meleeHitboxPrefabs.Count > 0)
        {
            actions.Add(new MeleeAttack(meleeHitboxPrefabs[0], this.transform));
            meleeSettings = new MeleeSettings(1.0f);
        }
        if (projectilePrefabs.Count > 0)
        {
            actions.Add(new FireProjectiles(projectilePrefabs[0], this.transform));
            projSettings = new ProjectileSettings(1, 1.0f, 3.0f, 5.0f);
            if (this.transform.childCount > 0)
            {
                projSettings.SetSpawnLocation(this.transform.GetChild(0));
            }
        }
        
        if (player == null)
        {
            if (FindFirstObjectByType<PlayerMovement>() != null)
            {
                player = FindFirstObjectByType<PlayerMovement>().gameObject;
            }
        }
    }

    //Going to need to time everything to make sure it looks correct
    protected override float Act()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        animator.SetBool("IsWalking", false);
        switch (state)
        {
            case SteamRexState.Spawn:
                currentAction = actions[0];
                currentAction.SetActionDuration(0.5f);
                state = SteamRexState.Phase1;
                break;
            case SteamRexState.Phase1:
                currentAction.SetActionDuration(1.0f);
                //double check range here (Melee Attack)
                if (player != null && (player.transform.position - this.transform.position).magnitude <= 0.5f && !hasUsedMelee)
                {
                    hasUsedMelee = true;
                    currentLocationTarget = player.transform.position;
                    animator.SetTrigger("StartMelee");
                }
                else
                {
                    hasUsedMelee = false;
                    if (currentCycleIndex == 0)
                    {
                        Phase1Charge();
                        currentCycleIndex++;
                    }
                    else if (currentCycleIndex == 1)
                    {
                        animator.SetTrigger("StartFire");    
                        currentCycleIndex++;
                    }
                    else if (currentCycleIndex == 2)
                    {
                        animator.SetTrigger("StartJump");
                        currentCycleIndex = 0;
                    }
                }
                if (currentHealth <= maxHealth * 0.65f)
                {
                    state = SteamRexState.Phase2;
                }
                break;
            case SteamRexState.Phase2:
                if (currentHealth <= maxHealth * 0.25f)
                {
                    state = SteamRexState.Phase3;
                }
                break;
            case SteamRexState.Phase3:
                if (currentHealth <= 0)
                {
                    state = SteamRexState.Dead;
                    StopAllCoroutines();
                    currentAction = null;
                }
                break;
            case SteamRexState.Dead:
                Destroy(this.gameObject);
                break;
        }
        if (currentAction == null)
        {
            return 1.0f;
        }
        return currentAction.Duration;
    }

    public void MeleeAttack()
    {
        currentAction = actions[1];
        //check location, lifetime, and hitbox size
        try
        {
            ConvertActionTo<MeleeAttack>(currentAction).AttackLocation(currentLocationTarget, meleeSettings);
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a MeleeAttack action in MeleeAttack()");
        }
    }

    public void Phase1Charge()
    {
        currentAction = actions[0];
        if (player != null)
        {
            if (player.transform.position.x <= this.transform.position.x)
            {
                currentLocationTarget = arenaMarks[0];
                TurnLeft();
            }
            else
            {
                currentLocationTarget = arenaMarks[1];
                TurnRight();
            }
        }
        else
        {
            int i = UnityEngine.Random.Range(0, 2);
            currentLocationTarget = arenaMarks[i];
            if (i == 0)
            {
                TurnLeft();
            }
            else
            {
                TurnRight();
            }
        }
        try
        {
            //there either should be something to tell it to trigger after the charge or have a charging animation to use
            StartCoroutine(ConvertActionTo<Move>(currentAction).ChargeTo(0.5f, currentLocationTarget));
            animator.SetBool("IsWalking", true);
            animator.SetTrigger("StartWalk");
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a Move action in Phase1Charge()");
        }
    }

    public void Phase1Jump()
    {
        currentAction = actions[0];
        currentLocationTarget = arenaMarks[2];
        if (currentLocationTarget.x < this.transform.position.x)
        {
            TurnLeft();
        }
        else
        {
            TurnRight();
        }
        try
        {
            StartCoroutine(ConvertActionTo<Move>(currentAction).JumpTo(currentLocationTarget));
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a Move action in Phase1Jump()");
        }
    }

    public void Phase1Fireball()
    {
        currentAction = actions[2];
        if (player != null)
        {
            currentLocationTarget = (Vector2)(player.transform.position - this.transform.position).normalized;
        }
        else
        {
            currentLocationTarget = (Vector2)(Vector3.zero - this.transform.position).normalized;
        }
        if (currentLocationTarget.x < this.transform.position.x)
        {
            TurnLeft();
        }
        else
        {
            TurnRight();
        }
        try
        {
            ConvertActionTo<FireProjectiles>(currentAction).EmitProjectile(projSettings, currentLocationTarget);
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a FireProjectiles action in Phase1Fireball()");
        }
    }


    //Probably should make sure it can only get hit by an attack once
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerAttack pa;
        if (collision.tag == "WeaponHitBox" && (pa = collision.GetComponent<PlayerAttack>()) != null)
        {
            sr.color = new Color(255, 230, 230);
            //TakeDamage(pa.damage);
        }
    }

    private void TurnLeft()
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    }

    private void TurnRight()
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check if its a wall, if it is:
        //StopAllCoroutines();
    }
}
