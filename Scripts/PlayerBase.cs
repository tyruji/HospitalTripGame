using Godot;
using System;
using static CharacterStates;

public abstract partial class PlayerBase : CharacterBody2D, IStateHolder, IMoveable, IAttacker
{
	[Export]
	public float Speed { get; set; } = 200f;

	[Export]
	public Vector2 Direction { get; set; } = Vector2.Zero;

	public bool AttackAtEnd { get; set; } = true;

	[Export]
	public Vector2 PreviousDirection { get; set; } = Vector2.Zero;

	public bool CanAttack { get; set; } = false;

	public bool Attacking { get; set; }

	public bool HoldingGun { get; set; }

	public Vector2 AttackDirection { get; set; }

	public State State { get; set; } = IDLE_STATE;

	public override void _Process(double delta)
	{
		State.Handle( this );
	}

	public override void _PhysicsProcess( double delta )
	{
		MoveAndSlide();
	}
	
	public virtual void Shoot()
	{
		
	}
}
