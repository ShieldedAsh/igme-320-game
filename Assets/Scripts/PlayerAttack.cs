using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private InputAction attackAction;

    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackDuration = 0.15f;
    [SerializeField] private float attackOffset = 0.75f;

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
       
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        mousePosition.z = transform.position.z;

        // Direction from player toward mouse
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Position the hitbox in front of the player
        Vector3 spawnPosition = transform.position + direction * attackOffset;

        // Rotate the hitbox toward the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        // Spawn hitbox as a child of the player
        GameObject hitbox = Instantiate(
            attackHitbox,
            spawnPosition,
            rotation
        );

        hitbox.transform.SetParent(transform);

        Destroy(hitbox, attackDuration);
    }
}