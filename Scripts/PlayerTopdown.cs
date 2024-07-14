using Godot;
using System;
using static CharacterStates;

public partial class PlayerTopdown : PlayerBase, IAnimatable
{
	[Export]
	public AnimationPlayer AnimationPlayer { get; private set; } = null;

	[Export]
	public Sprite2D Sprite { get; set; } = null;

	private Vector2 _force = Vector2.Zero;

	private float _bounceTime = 1;
	private float _timer = 0;

	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>( "AnimationPlayer" );
		Sprite = GetNode<Sprite2D>( "Sprite2D" );
	}

	public void AddForceImpulse( Vector2 force, float bounce_time )
	{
		_force += force;
		
		_bounceTime = bounce_time;
		_timer = 0;
	}
	
	public override void _Process( double delta )
	{
		base._Process( delta );

		_timer += ( float ) delta;
		_force = _force.Lerp( Vector2.Zero, _timer / _bounceTime );

		if( _timer > _bounceTime )
		{
			_force = Vector2.Zero;
			return;
		}
		

		Velocity += _force;
	}
}


