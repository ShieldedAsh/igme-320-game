using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttack : MonoBehaviour
{

    private InputAction attackAction;

    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDuration = 0.15f;

    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        if (attackAction.WasPressedThisFrame())
        {
            Attack();
        }
    }

    void Attack()
    {
        GameObject hitbox = Instantiate(
            attackHitbox,
            attackPoint.position,
            attackPoint.rotation
        );

        Destroy(hitbox, attackDuration);
    }
}
