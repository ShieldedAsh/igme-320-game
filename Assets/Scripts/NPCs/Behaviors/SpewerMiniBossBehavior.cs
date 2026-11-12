using System;
using UnityEngine;

enum SpewerMiniBossState {
    Spawn,
    Alive,
    Dead
}

enum MoveDirection
{
    Horizontal,
    Vertical
}

public class SpewerMiniBossBehavior : NPCBaseBehavior
{
    SpewerMiniBossState state;
    Vector2[] arenaMarkers;
    int currentMarkerIndex;
    Vector2 targetLocation;

    protected override void Start()
    {
        base.Start();
        projectileSettings.Add(new ProjectileSettings(0.25f, 4.0f, 6.0f, this.transform.GetChild(0).gameObject));
        actions.Add(new FireProjectiles(projectilePrefabs[0], this.transform, projectileSettings[0], OverwriteActionCooldown));
        moveSettings.Add(new MoveSettings(1.0f, 5.0f, 0.5f, 0.0f, false, false, EaseType.FunctionDefault));
        actions.Add(new Move(this.gameObject.transform, OverwriteActionCooldown, moveSettings[0]));
        actions[1].SetDelayTime(0.1f);

        arenaMarkers = new Vector2[4];
        arenaMarkers[0] = new Vector2(0.0f, -3.0f); 
        arenaMarkers[1] = new Vector2(-10.0f, 1.0f); 
        arenaMarkers[2] = new Vector2(0.0f, 5.0f);
        arenaMarkers[3] = new Vector2(10.0f, 1.0f);
        currentMarkerIndex = 0;
    }

    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        sr.color = new Color(1f, 0.7f, 0.7f, 1f);
    }

    protected override float Act()
    {
        switch (state)
        {
            case SpewerMiniBossState.Spawn:
                SpawnAct();
                break;
            case SpewerMiniBossState.Alive:
                AliveAct();
                break;
            case SpewerMiniBossState.Dead:
                DeadAct();
                break;
        }


        if (currentAction != null)
        {
            return currentAction.Duration;
        }
        return 1.0f;
    }

    private void SpawnAct()
    {
        currentAction = actions[0];
        currentHealth = maxHealth;
        phaseEvents = 3;

        /*
        this.transform.position = arenaMarkers[2];
        FlipDown();
        currentMarkerIndex = 3;
        */
        this.transform.position = arenaMarkers[0];
        currentMarkerIndex = 1;

        OverwriteActionCooldown(1.0f);
        currentPhaseEvent = 2;
        state = SpewerMiniBossState.Alive;
    }

    private void AliveAct()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }
        if (currentPhaseEvent == 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("StartFire");
            }
        }
        if (currentPhaseEvent == 1)
        {
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
                animator.SetTrigger("StartWalk");
            }
            Crawl();
        }
        if (currentPhaseEvent == 2)
        {
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
                animator.SetTrigger("StartWalk");
            }
            Crawl();
            currentMarkerIndex++;
            if (currentMarkerIndex >= 4)
            {
                currentMarkerIndex = 0;
            }
        }
        if (currentHealth <= 0)
        {
            state = SpewerMiniBossState.Dead;
        }
    }

    private void DeadAct()
    {
        AddAugment(0);
        Destroy(this.gameObject);
    }

    public void Spew()
    {
        currentAction = actions[0];
        currentAction.SetDelayTime(1.0f);
        if (player != null)
        {
            projectiles.AddRange(ConvertActionTo<FireProjectiles>(currentAction).EmitArc(90, AngleMath.GetVectorDirection(player.transform.position - projectileSettings[0].LocationObject.transform.position), 5));
        }
        else
        {
            projectiles.AddRange(ConvertActionTo<FireProjectiles>(currentAction).EmitArc(90, AngleMath.GetVectorDirection((Vector3)Vector2.zero - projectileSettings[0].LocationObject.transform.position), 5));
        }
        animator.SetTrigger("EndFire");
    }

    private void Crawl()
    {
        currentAction = actions[1];
        if (currentPhaseEvent == 1)
        {
            currentAction.SetDelayTime(0.1f);
        }
        else
        {
            currentAction.SetDelayTime(1.0f);
            switch (currentMarkerIndex)
            {
                case 0:
                    FlipUp();
                    break;
                case 1:
                    FlipRight();
                    break;
                case 2:
                    FlipDown();
                    break;
                case 3:
                    FlipLeft();
                    break;
            }
        }
        if ((currentMarkerIndex % 2 == 0 && currentPhaseEvent == 1) || (currentMarkerIndex % 2 == 1 && currentPhaseEvent == 2))
        {
            targetLocation = new Vector3(this.transform.position.x, arenaMarkers[currentMarkerIndex].y);
            currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedMove<Vector2>(targetLocation);
        }
        else
        {
            targetLocation = new Vector3(arenaMarkers[currentMarkerIndex].x, this.transform.position.y);
            currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedMove<Vector2>(targetLocation);
        }
        StartCoroutine(currentCoroutine);
    }

    private void OnDrawGizmos()
    {
        if (actions != null && actions.Count > 0)
        {
            if (currentPhaseEvent == 1)
            {
                Gizmos.color = Color.red;
                Vector2 target = Vector2.zero;
                if (player != null)
                {
                    target = player.transform.position;
                }
                Gizmos.DrawWireSphere(target, 0.25f);
                if (projectileSettings != null && projectileSettings.Count > 0)
                {
                    Gizmos.DrawLine(projectileSettings[0].LocationObject.transform.position, target);
                }
                else
                {
                    Gizmos.DrawLine(this.transform.position, target);
                }
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(targetLocation, 1);
            }
        }
    }
}
