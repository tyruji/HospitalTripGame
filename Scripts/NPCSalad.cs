using Godot;
using System;

public partial class NPCSalad : NPCTopdown, IAttacker
{
	public Salad Salad { get; set; }
	
	public bool CanAttack { get; set; } = true;
	
	public bool AttackAtEnd { get; set; } = true;
	
	public bool Attacking { get; set; }

	public bool HoldingGun { get; set; }

	public Vector2 AttackDirection { get; set; }
	
	public void Shoot()
	{
		
	}
}
