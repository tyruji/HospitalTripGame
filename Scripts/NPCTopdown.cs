using Godot;
using System;
using static CharacterStates;

public partial class NPCTopdown : CharacterBody2D, IMoveable, IStateHolder, IAnimatable
{
	// Integrate like a NavMesh agent and shit here
	// make functions to move the npc to a chosen position

	[Export]
	public float Speed { get; set; } = 200f;

	[Export]
	public Path2D Path { get; set; } = null;

	[Export]
	public float MinPointDistance { get; set; } = 50f;

	[Export]
	public AnimationPlayer AnimationPlayer { get; private set; } = null;

	[Export]
	public Sprite2D Sprite { get; set; } = null;

	public Vector2 TargetPosition { get => _navAgent.TargetPosition; set => _navAgent.TargetPosition = value;}

	public Vector2 Direction { get; set; }

	public Vector2 PreviousDirection { get; set; }

	public State State { get; set; } = IDLE_STATE;

	protected NavigationAgent2D _navAgent = null;

	private int _currentPointIdx = 0;

	private Vector2 _force = Vector2.Zero;

	private float _bounceTime = 1;

	private float _timer = 0;

	public override void _Ready()
	{
		_navAgent = GetNode<NavigationAgent2D>( "NavigationAgent2D" );
		AnimationPlayer = GetNode<AnimationPlayer>( "AnimationPlayer" );
		Sprite = GetNode<Sprite2D>( "Sprite2D" );
	}

	public override void _Process( double delta )
	{
		HandlePathFollow();
		
		if( GlobalPosition.DistanceSquaredTo( TargetPosition ) < MinPointDistance * MinPointDistance )
		{
			Direction = Vector2.Zero;
		}
		else
		{
			Direction = ToLocal( _navAgent.GetNextPathPosition() ).Normalized();
		}

		Velocity = Speed * Direction;

		State.Handle( this );
		
		HandleForces( delta );
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void AddForceImpulse( Vector2 force, float bounce_time )
	{
		_force += force;
		_bounceTime = bounce_time;
		_timer = 0;
	}
	
	private void HandleForces( double delta )
	{
		_timer += ( float ) delta;
		_force = _force.Lerp( Vector2.Zero, _timer / _bounceTime );
		if( _timer > _bounceTime )
		{
			_force = Vector2.Zero;
			return;
		}
		Velocity += _force;
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
