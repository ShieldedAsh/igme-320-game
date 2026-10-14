using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileBase : MonoBehaviour
{
    Vector2 movementDirection;
    float speed;
    float totalLifetime;
    float currentLifetime;

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

    void FixedUpdate()
    {
        currentLifetime += Time.fixedDeltaTime;
        if (movementDirection != Vector2.zero && speed != 0)
        {
            this.transform.position += (Vector3)(movementDirection * speed * Time.fixedDeltaTime);
        }
        if (currentLifetime >= totalLifetime)
        {
            NPCBaseBehavior parentBehavior;
            if ((parentBehavior = this.transform.parent.GetComponent<NPCBaseBehavior>()) != null)
            {
                parentBehavior.RemoveProjectile(this.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
