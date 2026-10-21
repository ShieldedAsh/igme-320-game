using UnityEngine;

public class TargetDummyBehavior : NPCBaseBehavior
{
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WeaponHitBox")
        {
            PlayerAttack pa;
            if ((pa = collision.gameObject.transform.parent.transform.GetComponent<PlayerAttack>()) != null)
            {
                TakeDamage(pa.damage);
            }
            
        }
    }

    protected override void Start()
    {
        maxHealth = 5;
        currentHealth = maxHealth;
    }

    //Logic for when the dummy takes damage
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        animator.SetTrigger("isDamaged");
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
    }

    protected override float Act()
    {
        return 1.0f;
    }
}
