using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    //input actions
    private InputAction moveAction;
    private InputAction jumpAction;

    //movement vars
    private Vector2 moveValue;
    [SerializeField]
    private float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveValue * Time.fixedDeltaTime * speed);
    }
}
