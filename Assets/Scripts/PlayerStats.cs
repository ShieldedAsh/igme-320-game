using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int playerHealth = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
         if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyProjectile")
         {
             playerHealth--;
             Debug.Log(playerHealth);
         }
    }
}
