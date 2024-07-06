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

		Vector2 dir = Direction;

		if( !IsOnFloor() )
		{
			_y_velocity += gravity * ( float ) delta;
		}
		else
		{
			_y_velocity = 0.0f;
		}

		if( Input.IsActionJustPressed( "jump" ) && IsOnFloor() )
		{
			_y_velocity = -JumpVelocity;
		}

			// Override the velocity for physics
			// and direction for animation handlers
			// so you dont have to create any new character states
		dir.Y = velocity.Y = _y_velocity;
		Direction = dir;

			// This removes any horizontal normalisation,
			// so the player can achieve the horizontal 
			// walking speed when jumping as well.
		if( velocity.X > 0 ) velocity.X = Speed;
		if( velocity.X < 0 ) velocity.X = -Speed;

		Velocity = velocity;

		MoveAndSlide();
	}
}
