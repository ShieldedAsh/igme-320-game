using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Health Up")]
public class HealthUpAugment : AugmentBase
{
    private int healthIncrease = 1;

    public override void ModifyStats(StatBlock stats)
    {
        stats.MaxHealth.Add += healthIncrease;
    }
}
