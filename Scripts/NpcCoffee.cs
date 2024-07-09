using Godot;
using System;

public partial class NpcCoffee : NPCTopdown
{
	[Export]
	public float PlayerBounceValue = 50f;
	
	public bool InQueue => TargetQueueIndex >= 0;
	
	public int TargetQeueueIndex { get; set; } = -1;
	
	private Area2D _area = null;
	
	public override _Ready()
	{
		_area = GetNode<Area2D>( "Area2D" );
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if( !InQueue || body is not PlayerTopdown player ) return;
		
		var dir = ( player.GlobalPosition - GlobalPosition ).Normalized();
		
		GD.Print( "BOUNCE PLAYER" );
			// Ta funkcje musisz jeszcze napisac w kodzie gracza
		player.AddForce( PlayerBounceValue * dir );
	}
}



