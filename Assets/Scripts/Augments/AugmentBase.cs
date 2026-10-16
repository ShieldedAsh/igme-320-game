using UnityEngine;

[CreateAssetMenu(fileName = "AugmentBase", menuName = "Scriptable Objects/AugmentBase")]
public class AugmentBase : ScriptableObject
{
	//name of the augment
    public string augName;

    public virtual void ModifyStats(StatBlock stats) {}
    public virtual void OnAdded(PlayerAugments owner) {}
    public virtual void OnRemoved(PlayerAugments owner){}
}
