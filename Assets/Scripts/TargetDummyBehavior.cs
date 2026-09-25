using UnityEngine;

public class TargetDummyEnemy : BaseBehavior
{
    SpriteRenderer sr;
    Color color;

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

        // Health Render variables
        sr = gameObject.GetComponent<SpriteRenderer>();
        color = sr.color;
    }

    //DisplayHealth() should be in BaseBehavior TakeDamage()/Update() or not exist
    protected override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            //Placeholder heal effect
            sr.color = Color.green;
        }
        else
        {
            //Should be replaced with damage animation
            sr.color = Color.red;
        }
        DisplayHealth();
    }
    
    //This should be empty, currently being used for accessing update
    //Probably should add something for misc. update things
    protected override void ChooseAction()
    {
        
        if (sr.color != color)
        {
            sr.color = color;
        }
    }

    //Temp, check if displaying health
    // Placeholder if we are displaying health
    private void DisplayHealth()
    {
        foreach (Transform childT in transform.GetChild(0).transform)
        {
            if (childT.name == "CurrentHP")
            {
                childT.localScale = new Vector3(currentHealth / (float)maxHealth, childT.localScale.y, childT.localScale.z);
            }
        }
    }
}
