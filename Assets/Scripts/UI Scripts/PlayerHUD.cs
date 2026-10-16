using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Image jetpackFuelFill;
    [SerializeField] private float maxFuel = 1f;

    private void Update()
    {
        jetpackFuelFill.fillAmount = Mathf.Clamp01(playerMovement.jpackFuel / maxFuel);
    }
}