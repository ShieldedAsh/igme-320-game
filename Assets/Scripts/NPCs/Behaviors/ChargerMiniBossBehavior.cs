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
    Vector2 currentTarget;


    protected override void Start()
    {
        base.Start();
        state = ChargerMiniBossState.Spawn;
        moveSettings.Add(new MoveSettings(1.0f, 10.0f, 1.0f, 0.0f, false, true, EaseType.FunctionDefault));
        moveSettings.Add(new MoveSettings(1.0f, 25.0f, 2.0f, 0.0f, true, false, EaseType.FunctionDefault));
        moveSettings.Add(new MoveSettings(1.0f, 10.0f, 1.0f, 1.0f, false, false, EaseType.FunctionDefault));
        moveSettings.Add(new MoveSettings(1.0f, 20.0f, 0.25f, 0.0f, false, false, EaseType.FunctionDefault));
        actions.Add(new Move(this.gameObject.transform, OverwriteActionCooldown, moveSettings[0]));
        arenaMarkers = new Vector2[3];
        arenaMarkers[0] = new Vector2(-7.2f, -2);
        arenaMarkers[1] = new Vector2(7.2f, -2);
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
        Debug.Log(currentAction.Duration);
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
        currentAction.SetDelayTime(1.0f);
        currentHealth = maxHealth;
        phaseEvents = 5;
        currentPhaseEvent = 4;
        this.transform.position = arenaMarkers[1];
        state = ChargerMiniBossState.Alive;
    }

    private void AliveAct()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsBoosting", false);
        }
        if (currentPhaseEvent == 0)
        {
            currentAction.SetDelayTime(1.0f);
            currentAction.SetActionDuration(2.0f);
            animator.SetTrigger("StartWalk");
        }
        if (currentPhaseEvent == 1)
        {
            currentAction.SetDelayTime(1.0f);
            animator.SetTrigger("StartWalk");
        }
        if (currentPhaseEvent == 2)
        {
            currentAction.SetActionDuration(1.0f);
            animator.SetTrigger("StartJump");
            JumpUp();
        }
        if (currentPhaseEvent == 3)
        {
            currentAction.SetDelayTime(0.5f);
            currentAction.SetActionDuration(1.0f);
            animator.SetTrigger("StartBoost");
        }
        if (currentPhaseEvent == 4)
        {
            currentAction.SetDelayTime(1.0f);
            animator.SetTrigger("StartBoost");
        }
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

    public void PickCrouchAction()
    {
        if (currentPhaseEvent == 1 || currentPhaseEvent == 2)
        {
            Charge();
        }
        else
        {
            Boost();
        }
    }

    private void Charge()
    {
        currentTarget = Vector2.zero;
        if (Mathf.Abs(arenaMarkers[0].x - this.gameObject.transform.position.x) > Mathf.Abs(arenaMarkers[1].x - this.gameObject.transform.position.x))
        {
            currentTarget = arenaMarkers[0];
            Debug.Log("Charging left");
        }
        else
        {
            currentTarget = arenaMarkers[1];
            Debug.Log("Charging right");
        }
        currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedCharge<Vector2>(currentTarget, new System.Tuple<Animator, string>(animator, "IsWalking"));
        TurnTowards(currentTarget);
        StartCoroutine(currentCoroutine);
    }

    private void Boost()
    {
        if (currentPhaseEvent == 3)
        {
            
            if (player != null)
            {
                currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedCharge<GameObject>(player, moveSettings[2], new System.Tuple<Animator, string>(animator, "IsBoosting"));
                TurnTowards(player.transform.position);
                Debug.Log("Boosting to player");
            }
            else
            {
                currentTarget = arenaMarkers[2];
                currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedCharge<Vector2>(currentTarget, moveSettings[2], new System.Tuple<Animator, string>(animator, "IsBoosting"));
                Debug.Log("Boosting Center");
                TurnTowards(currentTarget);
            }
        }
        else
        {
            if (Mathf.Abs(arenaMarkers[0].x - this.gameObject.transform.position.x) > Mathf.Abs(arenaMarkers[1].x - this.gameObject.transform.position.x))
            {
                currentTarget = arenaMarkers[1];
                Debug.Log("Boosting Right");
            }
            else
            {
                currentTarget = arenaMarkers[0];
                Debug.Log("Boosting Left");
            }
            currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedCharge<Vector2>(currentTarget, moveSettings[3], new System.Tuple<Animator, string>(animator, "IsBoosting"));
            TurnTowards(currentTarget);
        }
        StartCoroutine(currentCoroutine);
    }   

    private void JumpUp()
    {
        currentAction.SetDelayTime(1.0f);
        Debug.Log("Jumping");
        currentTarget = new Vector2(this.transform.position.x, 2);
        currentCoroutine = ConvertActionTo<Move>(currentAction).UntimedCharge<Vector2>(currentTarget, moveSettings[1]);
        TurnTowards(currentTarget);
        StartCoroutine(currentCoroutine);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        if (actions != null && actions.Count > 0)
        {
            if (currentPhaseEvent == 3 && player != null)
            {
                Gizmos.DrawWireSphere(player.transform.position, 0.5f);
            }
            else
            {
                Gizmos.DrawWireSphere(currentTarget, 0.5f);
            } 
        }
    }
}
