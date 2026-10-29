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
        moveSettings.Add(new MoveSettings(3.0f, 0.0f, 0.0f));
        moveSettings.Add(new MoveSettings(0.5f, 0.0f, 0.0f, 0.0f, false, false, EaseType.None));
        actions.Add(new Move(this.transform, OverwriteActionCooldown, moveSettings[0]));
        actions[0].SetDelayTime(1.0f);
        arenaMarkers = new Vector2[2];
        arenaMarkers[0] = new Vector2(0, 5);
        arenaMarkers[1] = new Vector2(0, -3);
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
        currentHealth = maxHealth;
        phaseEvents = 3;
        state = JumperMiniBossState.Alive;
    }

    private void AliveAct()
    {
        if (animator != null)
        {
            animator.SetTrigger("StartJump");
        }
        if (currentPhaseEvent == 0)
        {
            currentAction.SetDelayTime(1.0f);
            currentCoroutine = ConvertActionTo<Move>(currentAction).JumpToSafe<GameObject>(player);
            StartCoroutine(currentCoroutine);
        }
        if (currentPhaseEvent == 1)
        {
            currentAction.SetDelayTime(0.5f);
            currentCoroutine = ConvertActionTo<Move>(currentAction).JumpTo<Vector2>(arenaMarkers[0]);
            StartCoroutine(currentCoroutine);
        }
        if (currentPhaseEvent == 2)
        {
            currentAction.SetDelayTime(1.0f);
            currentCoroutine = ConvertActionTo<Move>(currentAction).TimedMove<Vector2>(arenaMarkers[1], moveSettings[1]);
            StartCoroutine(currentCoroutine);
        }
        if (currentHealth <= 0)
        {
            state = JumperMiniBossState.Dead;
        }
    }

    private void DeadAct()
    {
        //AugmentManager.DropAugment(1, true);
        Destroy(this.gameObject);
    }
}
