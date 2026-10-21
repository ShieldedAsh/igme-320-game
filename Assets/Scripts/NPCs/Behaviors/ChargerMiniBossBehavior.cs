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

    protected override void Start()
    {
        base.Start();
        state = ChargerMiniBossState.Spawn;
        actions.Add(new Move(this.gameObject.transform));
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
        phaseEvents = 0;
        state = ChargerMiniBossState.Alive;
    }

    private void AliveAct()
    {
        animator.SetBool("IsWalking", false);
        if (currentPhaseEvent == 0)
        {
            animator.SetTrigger("StartCharge");
            animator.SetBool("IsWalking", true);
            Charge(1.0f);
        }
        if (currentHealth <= 0)
        {
            state = ChargerMiniBossState.Dead;
        }
    }

    private void DeadAct()
    {
        Destroy(this.gameObject);
    }

    public void Charge(float chargeupTime)
    {
        StartCoroutine(ConvertActionTo<Move>(currentAction).ChargeInto(animator.GetCurrentAnimatorStateInfo(0).length + chargeupTime, player, 1.0f));
    }
}
