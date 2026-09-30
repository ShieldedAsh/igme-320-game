using UnityEngine;

public class TargetDummyBehavior : BaseBehavior
{

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Placeholder tag "WeaponHitBox" is used for weapon detection
        // Replace when actual weapon hit boxes are added
        if (collision.gameObject.tag == "WeaponHitBox")
        {
            // Damage should be read from the collider
            TakeDamage(1);
        }
    }

    protected override void Start()
    {
        maxHealth = 5;
        currentHealth = maxHealth;
    }

    //DisplayHealth() should be in BaseBehavior TakeDamage()/Update() or not exist
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
        else
        {
            animator.SetBool("isDamaged", true);
        }
    }
}
