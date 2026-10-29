using Unity.VisualScripting;
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

    protected override void Start()
    {
        base.Start();
        projectileSettings.Add(new ProjectileSettings(0.25f, 4.0f, 6.0f));
        actions.Add(new FireProjectiles(projectilePrefabs[0], this.transform, projectileSettings[0], OverwriteActionCooldown));
        moveSettings.Add(new MoveSettings(1.0f, 5.0f, 0.5f, 0.0f, true, false, EaseType.FunctionDefault));
        moveSettings.Add(new MoveSettings(1.0f, 5.0f, 0.5f, 0.0f, false, true, EaseType.FunctionDefault));
        actions.Add(new Move(this.gameObject.transform, OverwriteActionCooldown));
        actions[1].SetDelayTime(0.1f);

        arenaMarkers = new Vector2[4];
        arenaMarkers[0] = new Vector2(0, -3.35f); 
        arenaMarkers[1] = new Vector2(-9, 0); 
        arenaMarkers[2] = new Vector2(0, 4.3f);
        arenaMarkers[3] = new Vector2(9, 0);
        currentMarkerIndex = 0;
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
        phaseEvents = 3;
        this.transform.position = arenaMarkers[0];
        OverwriteActionCooldown(1.0f);
        currentPhaseEvent = 2;
        currentMarkerIndex = 1;
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
            Spew();
        }
        if (currentPhaseEvent == 1)
        {
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
                animator.SetTrigger("StartWalking");
            }
            if (currentMarkerIndex == 1 || currentMarkerIndex == 3)
            {
                Crawl(MoveDirection.Horizontal);
            }
            else
            {
                Crawl(MoveDirection.Vertical);
            }
        }
        if (currentPhaseEvent == 2)
        {
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
                animator.SetTrigger("StartWalking");
            }
            if (currentMarkerIndex == 1 || currentMarkerIndex == 3)
            {
                Crawl(MoveDirection.Vertical);
            }
            else
            {
                Crawl(MoveDirection.Horizontal);
            }
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
        //AugmentManager.DropAugment(1, true);
        Destroy(this.gameObject);
    }

    private void Spew()
    {
        currentAction = actions[0];
        if (player != null)
        {
            projectiles.AddRange(ConvertActionTo<FireProjectiles>(currentAction).EmitArc(45, AngleMath.GetVectorDirection(player.transform.position - this.transform.position), 5));
        }
        else
        {
            projectiles.AddRange(ConvertActionTo<FireProjectiles>(currentAction).EmitArc(45, AngleMath.GetVectorDirection((Vector3)Vector2.zero - this.transform.position), 5));
        }
    }

    private void Crawl(MoveDirection moveDirection)
    {
        currentAction = actions[1];
        int index = 0;
        if (moveDirection == MoveDirection.Horizontal)
        {
            index = 1;
            if (currentMarkerIndex == 0 || currentMarkerIndex == 1)
            {
                FlipUp();
            }
            else
            {
                FlipDown();
            }
        }
        else if (moveDirection == MoveDirection.Vertical) 
        {
            index = 0;
            if (currentMarkerIndex == 1)
            {
                FlipRight();
            }
            else
            {
                FlipLeft();
            }
        }
        TurnTowards(arenaMarkers[currentMarkerIndex]);
        currentCoroutine = ConvertActionTo<Move>(currentAction).TimedMove(arenaMarkers[currentMarkerIndex], moveSettings[index]);
        StartCoroutine(currentCoroutine);
    }
}
