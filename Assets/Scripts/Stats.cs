using UnityEngine;

public class Stats
{
	public float Add;

	//Resets all stat changes
	public void Reset()
	{
		Add = 0f;
	}

	//apply stat changes based on augments
    public float Apply(float baseVal) => (baseVal + Add);
    public int ApplyInt(int baseVal) => (Mathf.RoundToInt(Apply(baseVal)));
}

public class StatBlock
{
	//The stats themselves, Damage, Max Health, and MoveSpeed for now, can be changed later
	public readonly Stats Damage = new();
	public readonly Stats MaxHealth = new();
	public readonly Stats MoveSpeed = new();

	//Resets all stat changes for this block
	public void Reset()
	{
		Damage.Reset();
		MaxHealth.Reset();
		MoveSpeed.Reset();
	}
}
