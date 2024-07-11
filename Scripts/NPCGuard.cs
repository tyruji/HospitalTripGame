using Godot;
using System;

public partial class NPCGuard : NPCTopdown
{
	[Export]
	private Node2D _Sight = null;
	
	public override void _Process( double delta )
	{
		base._Process( delta );
		
		var dir = Direction;
		
		if( dir.LengthSquared() <= 0 ) return;
		
		if( dir.X > .33 )
		{
			_Sight.RotationDegrees = 180;
		}
		else if( dir.X < -.33 )
		{
			_Sight.RotationDegrees = 0;
		}
		else if( dir.Y < 0 )
		{
			_Sight.RotationDegrees = 90;
		}
		else if( dir.Y > 0 )
		{
			_Sight.RotationDegrees = 270;
		}
	}
}
