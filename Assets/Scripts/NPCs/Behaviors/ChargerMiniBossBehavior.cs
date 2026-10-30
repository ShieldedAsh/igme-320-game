using UnityEngine;

enum ChargerMiniBossState
{
    Spawn,
    Alive,
    Dead,
}

public class ChargerMiniBossBehavior : NPCBaseBehavior
{
    ChargerMiniBossState state;
    Vector2[] arenaMarkers;


    protected override void Start()
    {
        base.Start();
        state = ChargerMiniBossState.Spawn;
        moveSettings.Add(new MoveSettings(1.0f, 14.0f, 1.0f, 2.0f, false, true, EaseType.FunctionDefault));
        actions.Add(new Move(this.gameObject.transform, OverwriteActionCooldown, moveSettings[0]));
        arenaMarkers = new Vector2[3];
        arenaMarkers[0] = new Vector2(-7.2f, 0);
        arenaMarkers[1] = new Vector2(7.2f, 0);
        arenaMarkers[2] = new Vector2(0, -2);
    }

    protected override float Act()
    {
        // Pick Action
        switch (state)
        {
            case ChargerMiniBossState.Spawn:
                SpawnAct();
                break;
            case ChargerMiniBossState.Alive:
                AliveAct();
                break;
            case ChargerMiniBossState.Dead:
                DeadAct();
                break;
        }

        // Wait for...
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
        phaseEvents = 0;
        this.transform.position = arenaMarkers[2];
        state = ChargerMiniBossState.Alive;
    }

    private void AliveAct()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }
        Charge();
        if (currentHealth <= 0)
        {
            state = ChargerMiniBossState.Dead;
        }
    }

    private void DeadAct()
    {
        AddAugment(2);
        Destroy(this.gameObject);
    }

    public void Charge()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetTrigger("StartWalk");
        }
        //if (player != null)
        //{
        //    currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedCharge<GameObject>(player);
        //    TurnTowards(player.transform.position);
        //}
        //else
        //{
            Vector2 target = Vector2.zero;
            if (Mathf.Abs(arenaMarkers[0].x - this.gameObject.transform.position.x) > Mathf.Abs(arenaMarkers[1].x - this.gameObject.transform.position.x))
            {
                target = arenaMarkers[0];
            }
            else
            {
                target = arenaMarkers[1];
            }
            currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedCharge<Vector2>(target);
            TurnTowards(target);
        //}
        StartCoroutine(currentCoroutine);
    }
}
