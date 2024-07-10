using Godot;
using System;
using static CharacterStates;

public partial class NPCTopdown : CharacterBody2D, IMoveable, IStateHolder//, IAnimatable
{
	// Integrate like a NavMesh agent and shit here
	// make functions to move the npc to a chosen position

	[Export]
	public float Speed { get; set; } = 200f;

	[Export]
	public Path2D Path { get; set; } = null;

	[Export]
	public float MinPointDistance { get; set; } = 50f;

	public Vector2 TargetPosition { get => _navAgent.TargetPosition; set => _navAgent.TargetPosition = value;}

	public Vector2 Direction { get; set; }

	public Vector2 PreviousDirection { get; set; }

	public State State { get; set; } = IDLE_STATE;

	protected NavigationAgent2D _navAgent = null;

	protected bool _allowMovement = true;

	private int _currentPointIdx = 0;

	public override void _Ready()
	{
		_navAgent = GetNode<NavigationAgent2D>( "NavigationAgent2D" );

	}

	public override void _Process( double delta )
	{
		HandlePathFollow();
		
		if( _allowMovement )
		{
			Direction = ToLocal( _navAgent.GetNextPathPosition() ).Normalized();

			Velocity = Speed * Direction;
		}
		else
		{
			Direction = Vector2.Zero;
		}

		State.Handle( this );
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	private void HandlePathFollow()
	{
			// If there is no path to follow, dont do it
		if( Path == null || Path.Curve.PointCount == 0 ) return;

		Vector2 target = Path.Curve.GetPointPosition( _currentPointIdx );
	
		if( target.DistanceSquaredTo( GlobalPosition ) <= MinPointDistance * MinPointDistance )
		{
			_currentPointIdx = ( _currentPointIdx + 1) % Path.Curve.PointCount;
			target = Path.Curve.GetPointPosition( _currentPointIdx );
		}

		TargetPosition = target;
	}
}
