//sys imports
using System;
using System.Collections.Generic;

//Unity Imports
using UnityEngine;

public class PlayerAugments : MonoBehaviour
{
    //list of augment bases to apply
    private List<AugmentBase> augments = new();

    //the player stats (defaults are applied to this through their respective scripts)
    //Ex: PlayerStats.cs applies the base health, script names will prob need a refactor
    public StatBlock stats { get; } = new StatBlock();
    public event Action Changed;

    //Adds an augment
    public void Add(AugmentBase augment)
    {
        augments.Add(augment);
        augment.OnAdded(this);
        RecalculateStats();
    }

    //Remove an augment
    public void Remove(AugmentBase augment)
    {
        if (!augments.Remove(augment))
        {
            return;
        }

        augment.OnRemoved(this);
        RecalculateStats();
    }

    //Recheck stat changes on augment addition/removal
    private void RecalculateStats()
    {
        stats.Reset();
        foreach(AugmentBase a in augments)
        {
            a.ModifyStats(stats);
            Changed?.Invoke();
        }
    }
}
