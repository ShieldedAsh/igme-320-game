using NUnit.Framework;
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
    [SerializeField]
    SteamRexState state;
    bool hasUsedMelee;
    Vector2[] arenaMarks;
    Vector2 currentLocationTarget;
    [SerializeField]
    Sprite[] phaseStartSprites;

    protected override void Start()
    {
        base.Start();
        state = SteamRexState.Spawn;
        hasUsedMelee = false;
        sr.sprite = phaseStartSprites[0];
        actions.Add(new Move(this.transform, 2.0f));
        currentAction = actions[0];
        GameObject markers;
        if ((markers = GameObject.Find("RoomMarkers")) != null)
        {
            arenaMarks = new Vector2[markers.transform.childCount];
            for (int i = 0; i < markers.transform.childCount; i++)
            {
                arenaMarks[i] = markers.transform.GetChild(i).transform.position;
            }
        }
        else
        {
            arenaMarks = new Vector2[3];
            arenaMarks[0] = new Vector2(-7.35f, -4.0f); //left
            arenaMarks[1] = new Vector2(7.35f, -4.0f); //right
            arenaMarks[2] = new Vector2(0.0f, -4.0f); //center floor
        }
        currentLocationTarget = arenaMarks[0];
        if (meleeHitboxPrefabs.Count > 0)
        {
            actions.Add(new MeleeAttack(meleeHitboxPrefabs[0], this.transform));
            meleeSettings.Add(new MeleeSettings(0.33f, new Vector2(0.6f, 0.6f)));
        }
        if (projectilePrefabs.Count > 0)
        {
            actions.Add(new FireProjectiles(projectilePrefabs[0], this.transform));
            projectileSettings.Add(new ProjectileSettings(2.5f, 5.0f, 4.0f));
            if (this.transform.childCount > 0)
            {
                //projSettings.SetSpawnLocation(this.transform.GetChild(0));
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
        animator.SetBool("IsWalking", false);
        switch (state)
        {
            case SteamRexState.Spawn:
                currentAction = actions[0];
                currentAction.SetActionDuration(1.5f);
                state = SteamRexState.Phase1;
                animator.SetInteger("Phase", 1);
                phaseEvents = 4;
                break;
            case SteamRexState.Phase1:
                if (player != null && (player.transform.position - this.transform.position).magnitude <= 2.0f && !hasUsedMelee)
                {
                    currentAction.SetActionDuration(1.25f);
                    hasUsedMelee = true;
                    currentLocationTarget = (Vector2)this.transform.position + new Vector2(-0.35f, 0.35f);
                    animator.SetTrigger("StartMelee");
                }
                else
                {
                    hasUsedMelee = false;
                    if (currentPhaseEvent == 0)
                    {
                        currentAction.SetActionDuration(2.25f);
                        Phase1Charge();
                    }
                    else if (currentPhaseEvent == 1)
                    {
                        currentAction.SetActionDuration(0.5f);
                    }
                    else if (currentPhaseEvent == 2)
                    {
                        currentAction.SetActionDuration(1.25f);
                        animator.SetTrigger("StartFire");    
                    }
                    else if (currentPhaseEvent == 3)
                    {
                        currentAction.SetActionDuration(0.5f);
                    }
                    else if (currentPhaseEvent == 4)
                    {
                        currentAction.SetActionDuration(1.75f);
                        animator.SetTrigger("StartJump");
                    }
                }
                if (currentHealth <= maxHealth * 0.65f)
                {
                    state = SteamRexState.Phase2;
                    sr.sprite = phaseStartSprites[1];
                    //projSettings.SetCount(5);
                    projectileSettings[0].SetScale(2.0f);
                    projectileSettings[0].SetSpeed(3.5f);
                    animator.SetInteger("Phase", 2);
                    currentPhaseEvent = 6;
                }
                break;
            case SteamRexState.Phase2:
                if (player != null && (player.transform.position - this.transform.position).magnitude <= 2.0f && !hasUsedMelee)
                {
                    currentAction.SetActionDuration(1.0f);
                    hasUsedMelee = true;
                    currentLocationTarget = (Vector2)this.transform.position + new Vector2(-0.35f, 0.35f);
                    animator.SetTrigger("StartMelee");
                }
                else
                {
                    hasUsedMelee = false;
                    if (currentPhaseEvent == 0)
                    {
                        currentAction.SetActionDuration(2.05f);
                        Phase2Charge();
                    }
                    else if (currentPhaseEvent == 1)
                    {
                        currentAction.SetActionDuration(0.5f);
                    }
                    else if (currentPhaseEvent == 2)
                    {
                        currentAction.SetActionDuration(1.0f);
                        Debug.Log("AHHHH");
                        animator.SetTrigger("StartFire");
                    }
                    else if (currentPhaseEvent == 3)
                    {
                        currentAction.SetActionDuration(0.5f);
                    }
                    else if (currentPhaseEvent == 4)
                    {
                        currentAction.SetActionDuration(1.5f);
                        animator.SetTrigger("StartJump");
                    }
                    else if (currentPhaseEvent == 5)
                    {
                        currentAction.SetActionDuration(0.25f);
                    }
                    else if (currentPhaseEvent == 6)
                    {
                        currentAction.SetActionDuration(1.5f);
                        animator.SetTrigger("StartJump");
                    }
                }
                if (currentHealth <= maxHealth * 0.25f)
                {
                    state = SteamRexState.Phase3;
                    sr.sprite = phaseStartSprites[2];
                    //projSettings.SetCount(11);
                    projectileSettings[0].SetScale(1.5f);
                    projectileSettings[0].SetSpeed(3.5f);
                    animator.SetInteger("Phase", 3);
                    phaseEvents = 8;
                }
                break;
            case SteamRexState.Phase3:
                if (player != null && (player.transform.position - this.transform.position).magnitude <= 2.0f && !hasUsedMelee)
                {
                    currentAction.SetActionDuration(1.0f);
                    hasUsedMelee = true;
                    currentLocationTarget = (Vector2)this.transform.position + new Vector2(-0.35f, 0.35f);
                    animator.SetTrigger("StartMelee");
                }
                else
                {
                    hasUsedMelee = false;
                    if (currentPhaseEvent == 0)
                    {
                        currentAction.SetActionDuration(2.0f);
                        Phase3Charge();
                    }
                    else if (currentPhaseEvent == 1)
                    {
                        currentAction.SetActionDuration(0.5f);
                    }
                    else if (currentPhaseEvent == 2)
                    {
                        currentAction.SetActionDuration(0.75f);
                        animator.SetTrigger("StartFire");
                    }
                    else if (currentPhaseEvent == 3)
                    {
                        currentAction.SetActionDuration(0.5f);
                    }
                    else if (currentPhaseEvent == 4)
                    {
                        currentAction.SetActionDuration(1.25f);
                        animator.SetTrigger("StartJump");
                    }
                    else if (currentPhaseEvent == 5)
                    {
                        currentAction.SetActionDuration(0.5f);
                    }
                    else if (currentPhaseEvent == 6)
                    {
                        currentAction.SetActionDuration(1.25f);
                        animator.SetTrigger("StartJump");
                    }
                    else if (currentPhaseEvent == 7)
                    {
                        currentAction.SetActionDuration(0.5f);
                    }
                    else if (currentPhaseEvent == 8)
                    {
                        currentAction.SetActionDuration(1.25f);
                        animator.SetTrigger("StartJump");
                    }
                }
                if (currentHealth <= 0)
                {
                    state = SteamRexState.Dead;
                    animator.SetInteger("Phase", 4);
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
            projectiles.Add(ConvertActionTo<MeleeAttack>(currentAction).AttackLocation(this.transform.GetChild(1).transform.position, meleeSettings[0]));
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a MeleeAttack action in MeleeAttack()");
        }
    }

    /// <summary>
    /// Long Charge
    /// </summary>
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
            StartCoroutine(ConvertActionTo<Move>(currentAction).ChargeTo(0.75f, currentLocationTarget));
            animator.SetTrigger("StartWalk");
            animator.SetBool("IsWalking", true);
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a Move action in Phase1Charge()");
        }
    }

    /// <summary>
    /// Jump to Center
    /// </summary>
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

    /// <summary>
    /// Fire fireball towards player
    /// </summary>
    public void Phase1Fireball()
    {
        currentAction = actions[2];
        if (player != null)
        {
            currentLocationTarget = (Vector2)(player.transform.position - this.transform.GetChild(0).transform.position).normalized;
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
            projectiles.Add(ConvertActionTo<FireProjectiles>(currentAction).EmitProjectile(projectileSettings[0], currentLocationTarget));
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a FireProjectiles action in Phase1Fireball()");
        }
    }


    /// <summary>
    /// Shorter Charge
    /// </summary>
    public void Phase2Charge()
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
            StartCoroutine(ConvertActionTo<Move>(currentAction).ChargeTo(0.65f, currentLocationTarget));
            animator.SetTrigger("StartWalk");
            animator.SetBool("IsWalking", true);
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a Move action in Phase2Charge()");
        }
    }

    /// <summary>
    /// Jump to the player, then center
    /// </summary>
    public void Phase2Jump()
    {
        currentAction = actions[0];
        if (currentPhaseEvent == 5 )
        {
            currentLocationTarget = player.transform.position;
        }
        else
        {
            currentLocationTarget = arenaMarks[2];
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
            StartCoroutine(ConvertActionTo<Move>(currentAction).JumpTo(currentLocationTarget));
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a Move action in Phase2Jump()");
        }
    }

    /// <summary>
    /// Fire wide cone towards player
    /// </summary>
    public void Phase2Fireball()
    {
        currentAction = actions[2];
        if (player != null)
        {
            currentLocationTarget = (Vector2)(player.transform.position - this.transform.GetChild(0).transform.position).normalized;
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
            projectiles.AddRange(ConvertActionTo<FireProjectiles>(currentAction).EmitArc(90, projectileSettings[0], currentLocationTarget));
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a FireProjectiles action in Phase2Fireball()");
        }
    }

    /// <summary>
    /// Short Charge
    /// </summary>
    public void Phase3Charge()
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
            animator.SetTrigger("StartWalk");
            animator.SetBool("IsWalking", true);
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a Move action in Phase3Charge()");
        }
    }

    /// <summary>
    /// Jump to player twice, jump to center
    /// </summary>
    public void Phase3Jump()
    {
        currentAction = actions[0];
        //it doesn't trigger fast enough :tear:
        if (currentPhaseEvent == 5 || currentPhaseEvent == 7)
        {
            currentLocationTarget = player.transform.position;
        }
        else
        {
            currentLocationTarget = arenaMarks[2];
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
            StartCoroutine(ConvertActionTo<Move>(currentAction).JumpTo(currentLocationTarget));
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a Move action in Phase3Jump()");
        }
    }

    /// <summary>
    /// Fire wide cone with higher proj density towards player
    /// </summary>
    public void Phase3Fireball()
    {
        currentAction = actions[2];
        if (player != null)
        {
            currentLocationTarget = (Vector2)(player.transform.position - this.transform.GetChild(0).transform.position).normalized;
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
            projectiles.AddRange(ConvertActionTo<FireProjectiles>(currentAction).EmitArc(120, projectileSettings[0], currentLocationTarget));
        }
        catch (TypeAccessException)
        {
            Debug.Log($"Could not run {currentAction} as a FireProjectiles action in Phase3Fireball()");
        }
    }
}
