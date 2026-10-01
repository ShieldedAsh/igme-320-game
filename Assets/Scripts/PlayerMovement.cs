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
    private InputAction jetpackAction;

    //movement refs
    private float moveX;
    [SerializeField] private float speed = 5f;

    //jump refs
    private bool jumpPressed;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private LayerMask groundLayer;

    //jetpack refs
    [SerializeField] private float jetpackFuel = 1f;
    private bool jetpackInUse;
    [SerializeField] private float jetpackCooldown = 3f;
    [SerializeField] private float jetpackForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        jetpackAction = InputSystem.actions.FindAction("Jetpack");
    }

    void Update()
    {
        moveX = moveAction.ReadValue<Vector2>().x;

        if (jumpAction.WasPressedThisFrame())
        {
            jumpPressed = true;
        }

        if (jetpackAction.IsInProgress())
        {
            jetpackInUse = true;
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

        //if jetpack is being held
        if (jetpackInUse)
        {
            //and there is fuel
            if (jetpackFuel >= 0.0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jetpackForce);
                jetpackFuel -= Time.fixedDeltaTime;
            }
        }

        if (jetpackFuel <= 0.0f)
        {
            jetpackInUse = false;
        }

        if (IsGrounded() && jetpackFuel <= 0.0f)
        {
            jetpackCooldown -= Time.fixedDeltaTime;

            if (jetpackCooldown <= 0.0f)
            {
                jetpackFuel = 1f;
                jetpackCooldown = 3f;
            }
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