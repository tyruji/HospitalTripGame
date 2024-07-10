using Godot;
using System;

public partial class InputHandler : Node
{
	private PlayerBase _player = null;
	private Vector2 _rawDir = new Vector2();

	public override void _Ready()
	{
		_player = GetParent<PlayerBase>();
	}

	public override void _UnhandledInput( InputEvent @event )
	{
		if( @event.IsActionPressed( "up" ) || @event.IsActionReleased( "down" ) )
		{
			_rawDir += Vector2.Up;
		}
		if( @event.IsActionPressed( "down" ) || @event.IsActionReleased( "up" ) )
		{
			_rawDir += Vector2.Down;
		}
		if( @event.IsActionPressed( "right" ) || @event.IsActionReleased( "left" ) )
		{
			_rawDir += Vector2.Right;
		}
		if( @event.IsActionPressed( "left" ) || @event.IsActionReleased( "right" ) )
		{
			_rawDir += Vector2.Left;
		}

		var new_dir = _rawDir.Normalized();
		if( new_dir != _player.Direction )
		{
			_player.PreviousDirection = _player.Direction;
			_player.Direction = _rawDir.Normalized();
		}

		_player.Attacking = _player.Attacking != @event.IsActionPressed( "attack", true, true );
		if( _player.Attacking ) _player.PreviousDirection = _player.AttackDirection;

		_player.AttackDirection = ( _player.GetGlobalMousePosition() - _player.GlobalPosition ).Normalized();
	
			// IDK WHAT TO DO AND IF I WILL NEED THIS :)
		//_player.HoldingGun =  _player.HoldingGun != @event.IsActionPressed( "swap_weapons", true, true ) ;
	}
}
