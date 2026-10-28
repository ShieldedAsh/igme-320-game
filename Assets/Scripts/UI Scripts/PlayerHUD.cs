using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Image jetpackFuelFill;
    [SerializeField] private float maxFuel = 1f;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Image healthFill;

    private void Start()
    {
        BindUI();
        BindToPlayer();
    }

    private void BindUI()
    {
        Canvas hudCanvas = GetComponentInParent<Canvas>();

        Transform fuelTransform = hudCanvas.transform.Find("FuelBar/Fuel");
        Transform healthTransform =hudCanvas.transform.Find("HealthBar/Health");

        if (fuelTransform != null)
        {
            jetpackFuelFill = fuelTransform.GetComponent<Image>();
        }

        if (healthTransform != null)
        {
            healthFill = healthTransform.GetComponent<Image>();
        }
    }

    private void BindToPlayer()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (playerMovement != null)
        {
            playerMovement.TryGetComponent(out playerStats);
        }
    }

    private void Update()
    {
        if (playerMovement != null && jetpackFuelFill != null)
        {
            jetpackFuelFill.fillAmount = Mathf.Clamp01(playerMovement.jpackFuel / maxFuel);
        }

        if (playerStats != null && healthFill != null && playerStats.CurrentMaxHealth > 0)
        {
            healthFill.fillAmount = Mathf.Clamp01((float)playerStats.CurrentHealth /playerStats.CurrentMaxHealth);
        }
    }
}