using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int playerHealth = 3;
    private float lastHitTimer = 1.0f;
    private bool justHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (justHit && lastHitTimer > 0.0f)
        {
            lastHitTimer -= Time.deltaTime;
            // Debug.Log($"Timer: {lastHitTimer}");
        }

        if (lastHitTimer <= 0.0f)
        {
            justHit = false;
            lastHitTimer = 1.0f;
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if ((col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyProjectile") && !justHit)
        {
            //call projectile base class if it is a projectile and tell it to Destroy itself after the instance of damage occurs
            playerHealth--;
            Debug.Log(playerHealth);
            justHit = true;
        }
    }
}
