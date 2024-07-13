using System;
using Godot;

public interface IAttacker
{
	public bool CanAttack { get; set; }
	public bool Attacking { get; set; }
	public bool HoldingGun { get; set; }
	public Vector2 AttackDirection { get; set; }
	public bool AttackAtEnd { get; set; }
	public void Shoot();
}
