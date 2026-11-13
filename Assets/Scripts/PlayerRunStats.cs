using System.Collections.Generic;

public static class PlayerRunStats
{
    private static bool hasSavedData;
    private static int savedHealth;
    private static float savedJetpackFuel = 1f;

    public static void Capture(
        PlayerStats playerStats,
        PlayerMovement playerMovement)
    {
        if (playerStats == null)
        {
            return;
        }

        savedHealth = playerStats.CurrentHealth;

        if (playerMovement != null)
        {
            savedJetpackFuel = playerMovement.jpackFuel;
        }

        hasSavedData = true;
    }

    public static void Restore(PlayerStats playerStats,PlayerMovement playerMovement)
    {
        if (!hasSavedData || playerStats == null)
        {
            return;
        }


        playerStats.RestoreHealth(savedHealth);

        if (playerMovement != null)
        {
            playerMovement.RestoreJetpackFuel(savedJetpackFuel);
        }

        Clear();
    }

    public static void Clear()
    {
        hasSavedData = false;
        savedHealth = 0;
        savedJetpackFuel = 1f;
    }
}