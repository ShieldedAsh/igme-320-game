using UnityEngine;

enum JumperMiniBossState
{
    Spawn,
    Alive,
    Dead
}

public class JumperMiniBossBehavior : NPCBaseBehavior
{
    JumperMiniBossState state;
    Vector2[] arenaMarkers;

    protected override void Start()
    {
        base.Start();
        state = JumperMiniBossState.Spawn;
        moveSettings.Add(new MoveSettings(1.0f, 0.0f, 0.0f));
        moveSettings.Add(new MoveSettings(0.25f, 0.0f, 0.0f, 0.0f, false, false, EaseType.None));
        actions.Add(new Move(this.transform, OverwriteActionCooldown, moveSettings[0]));
        projectileSettings.Add(new ProjectileSettings(1.0f, 10.0f, 3.0f));
        actions.Add(new FireProjectiles(projectilePrefabs[0], this.transform, projectileSettings[0], OverwriteActionCooldown));
        actions[0].SetDelayTime(1.0f);
        arenaMarkers = new Vector2[4];
        arenaMarkers[0] = new Vector2(0, 2.75f);
        arenaMarkers[1] = new Vector2(0, -2.75f);
        arenaMarkers[2] = new Vector2(-8.5f, -2.75f);
        arenaMarkers[3] = new Vector2(8.5f, -2.75f);
    }

    protected override float Act()
    {
        switch (state)
        {
            case JumperMiniBossState.Spawn:
                SpawnAct();
                break;
            case JumperMiniBossState.Alive:
                AliveAct();
                break;
            case JumperMiniBossState.Dead:
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
        this.transform.position = arenaMarkers[1];
        currentHealth = maxHealth;
        phaseEvents = 4;
        currentPhaseEvent = 3;
        state = JumperMiniBossState.Alive;
    }

    private void AliveAct()
    {
        if (animator != null)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsSlamming", false);
        }
        if (currentPhaseEvent == 0)
        {
            if (animator != null)
            {
                animator.SetBool("IsJumping", true);
                animator.SetTrigger("StartJump");
            }
            currentAction = actions[0];
            currentAction.SetDelayTime(1.0f);
            if (player != null)
            {
                currentCoroutine = ConvertActionTo<Move>(currentAction).JumpToSafe<GameObject>(player);
                TurnTowards(player.transform.position);
            }
            else
            {
                int index = Random.Range(2, 4);
                currentCoroutine = ConvertActionTo<Move>(currentAction).JumpToSafe<Vector2>(arenaMarkers[index]);
                TurnTowards(arenaMarkers[index]);
            }
            
            StartCoroutine(currentCoroutine);
        }
        if (currentPhaseEvent == 1)
        {
            if (animator != null)
            {
                animator.SetBool("IsJumping", true);
                animator.SetTrigger("StartJump");
            }
            currentAction.SetDelayTime(0.25f);
            currentCoroutine = ConvertActionTo<Move>(currentAction).JumpTo<Vector2>(arenaMarkers[0]);
            TurnTowards(arenaMarkers[0]);
            StartCoroutine(currentCoroutine);
        }
        if (currentPhaseEvent == 2)
        {
            if (animator != null)
            {
                animator.SetBool("IsSlamming", true);
                animator.SetTrigger("StartSlam");
            }
            currentAction.SetDelayTime(0.0f);
            currentCoroutine = ConvertActionTo<Move>(currentAction).TimedMove<Vector2>(arenaMarkers[1], moveSettings[1]);
            StartCoroutine(currentCoroutine);
        }
        if (currentPhaseEvent == 3)
        {
            currentAction = actions[1];
            currentAction.SetDelayTime(1.0f);
            ConvertActionTo<FireProjectiles>(currentAction).EmitCircle(0, 2);
        }
        if (currentHealth <= 0)
        {
            state = JumperMiniBossState.Dead;
        }
    }

    private void DeadAct()
    {
        //PlayerAugments.Add()
        Destroy(this.gameObject);
    }
}
