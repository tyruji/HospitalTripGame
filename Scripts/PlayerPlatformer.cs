using Godot;
using System;

public partial class PlayerPlatformer : PlayerBase
{	
	[Export]
	public float JumpVelocity = 400.0f;

	public float gravity = ProjectSettings.GetSetting( "physics/2d/default_gravity" ).AsSingle();

	private float _y_velocity = 0;

	public override void _PhysicsProcess( double delta )
	{
		Vector2 velocity = Velocity;


		if( !IsOnFloor() )
			_y_velocity += gravity * ( float )delta;

		if( Input.IsActionJustPressed( "jump" ) && IsOnFloor() )
			_y_velocity = -JumpVelocity;

		velocity.Y = _y_velocity;
		Velocity = velocity;

		MoveAndSlide();
	}
}
