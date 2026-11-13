//sys imports
using System;
using System.Collections.Generic;

//Unity Imports
using UnityEngine;

public class PlayerAugments : MonoBehaviour
{
    //list of all augments that are NOT applied to the player
    //note: do we want augments to be able to be applied more than once?
    private List<AugmentBase> allAugments = new();
    public List<AugmentBase> AllAugments { get => allAugments;  }

    //list of augment bases to apply
    private List<AugmentBase> augments;

    //the player stats (defaults are applied to this through their respective scripts)
    //Ex: PlayerStats.cs applies the base health, script names will prob need a refactor
    public StatBlock stats { get; } = new StatBlock();
    public event Action Changed;

    private void Start()
    {
        //loads all potential augments to be added
        augments = new List<AugmentBase>(Resources.LoadAll<AugmentBase>("Assets/Scripts/Augments/UsableAugs"));
    }

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

    /// <summary>
    /// Adds a random augment from the allAugments list
    /// </summary>
    public void RandomAdd()
    {
        AugmentBase newAug = allAugments[UnityEngine.Random.Range(0, allAugments.Count)];

        allAugments.Remove(newAug);
        augments.Add(newAug);
    }
}
