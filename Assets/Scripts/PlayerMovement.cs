using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //component refs
    private Rigidbody2D rb;
    private Collider2D col;

    //movement actions
    private InputAction moveAction;
    private InputAction jumpAction;

    //movement refs
    private float moveX;
    [SerializeField] private float speed = 5f;

    //jump refs
    private bool jumpPressed;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        moveX = moveAction.ReadValue<Vector2>().x;

        if (jumpAction.WasPressedThisFrame())
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        //movement in x dir
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        if (jumpPressed)
        {
            //check if player is grounded
            bool grounded = IsGrounded();

            if (grounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            }

            jumpPressed = false;
        }
    }

    private bool IsGrounded()
    {
        //get bounds of collider
        Bounds colBounds = col.bounds;
        //send a raycast down
        RaycastHit2D hit = Physics2D.BoxCast(colBounds.center, colBounds.size, 0f, Vector2.down, 0.05f, groundLayer);
        //if hit collider isnt null, you hit something, which means you're grounded
        return hit.collider != null;
    }
}