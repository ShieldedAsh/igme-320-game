using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeBase : MonoBehaviour
{
    float totalLifetime;
    float currentLifetime;

    public float TotalLifetime { get => totalLifetime; }
    public float CurrentLifetime { get => currentLifetime; }

    public void SetLifetime(float _lifeTime)
    {
        totalLifetime = _lifeTime;
    }

    void FixedUpdate()
    {
        currentLifetime += Time.fixedDeltaTime;
        if (currentLifetime >= totalLifetime)
        {
            NPCBaseBehavior parentBehavior;
            if ((parentBehavior = this.transform.parent.GetComponent<NPCBaseBehavior>()) != null)
            {
                parentBehavior.RemoveProjectile(this.gameObject);
            }
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
