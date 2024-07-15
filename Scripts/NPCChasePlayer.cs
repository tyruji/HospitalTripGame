using Godot;
using System;

public partial class NPCChasePlayer : NPCTopdown
{
	[Export]
	private Node2D _Sight = null;
	
	private PlayerTopdown _player = null;
	
	public override void _Ready()
	{
		base._Ready();
		_player = ( PlayerTopdown ) GetTree().GetFirstNodeInGroup( "player" );
		_Sight = GetNode<Node2D>( "Sight" );
	}
	
	public override void _Process( double delta )
	{
		TargetPosition = _player.GlobalPosition;
		base._Process( delta );
	}
}
