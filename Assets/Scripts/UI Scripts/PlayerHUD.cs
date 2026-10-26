using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Image jetpackFuelFill;
    [SerializeField] private float maxFuel = 1f;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Image healthFill;

    private void Update()
    {
        jetpackFuelFill.fillAmount = Mathf.Clamp01(playerMovement.jpackFuel / maxFuel);

        if (playerStats.CurrentMaxHealth > 0)
        {
            healthFill.fillAmount = Mathf.Clamp01((float)playerStats.CurrentHealth / playerStats.CurrentMaxHealth);
        }
    }
}