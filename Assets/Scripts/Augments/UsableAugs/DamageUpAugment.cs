using UnityEngine;

[CreateAssetMenu(menuName = "Augments/Damage Up")]
public class DamageUpAugment : AugmentBase
{
    private int dmgIncrease = 1;

    public override void ModifyStats(StatBlock stats)
    {
        stats.Damage.Add += dmgIncrease;
    }
}
