using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileBase : MonoBehaviour
{
    Vector2 movementDirection;
    float speed;
    float totalLifetime;
    float currentLifetime;
    Transform parent;

    public Vector2 MovementDirection { get => movementDirection; }
    public float Speed { get => speed; }
    public float TotalLifetime { get => totalLifetime; }
    public float CurrentLifetime { get => currentLifetime; }

    public void SetDirection(Vector2 _direction)
    {
        movementDirection = _direction.normalized;
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public void SetLifetime(float _lifeTime)
    {
        totalLifetime = _lifeTime;
    }

    public void SetParent(Transform _parent)
    {
        parent = _parent;
    }

    void FixedUpdate()
    {
        currentLifetime += Time.fixedDeltaTime;
        if (movementDirection != Vector2.zero && speed != 0)
        {
            this.transform.position += (Vector3)(movementDirection * speed * Time.fixedDeltaTime);
        }
        if (currentLifetime >= totalLifetime)
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        if (parent != null)
        {
            NPCBaseBehavior parentBehavior;
            if ((parentBehavior = parent.GetComponent<NPCBaseBehavior>()) != null)
            {
                parentBehavior.RemoveProjectile(this.gameObject);
            }
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            DestroySelf();
        }
    }
}
