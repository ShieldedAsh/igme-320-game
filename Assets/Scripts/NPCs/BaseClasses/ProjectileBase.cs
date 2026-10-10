using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class ProjectileBase : MonoBehaviour
{
    [SerializeField]
    bool isPowerful = false;
    Vector2 movementDirection;
    float speed;
    float totalLifetime;
    float currentLifetime;

    public Vector2 MovementDirection { get => movementDirection; }
    public float Speed { get => speed; }
    public float TotalLifetime { get => totalLifetime; }
    public float CurrentLifetime { get => currentLifetime; }

    public void setDirection(Vector2 _direction)
    {
        movementDirection = _direction.normalized;
    }

    public void setSpeed(float _speed)
    {
        speed = _speed;
    }

    public void setLifetime(float _lifeTime)
    {
        totalLifetime = _lifeTime;
    }

    void FixedUpdate()
    {
        currentLifetime += Time.fixedDeltaTime;
        this.transform.position += (Vector3)(movementDirection * speed * Time.fixedDeltaTime);
        if (currentLifetime >= totalLifetime)
        {
            NPCBaseBehavior parentBehavior = null;
            if ((parentBehavior = this.transform.parent.GetComponent<NPCBaseBehavior>()) != null)
            {
                parentBehavior.RemoveProjectile(this.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (true)
        {
            if (isPowerful)
            {
                //damage 2
            }
            else
            {
                //damage 1
            }
        }
    }
}
