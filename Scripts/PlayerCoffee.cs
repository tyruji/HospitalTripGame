using Godot;
using System;

public partial class PlayerCoffee : PlayerTopdown
{
	[Export]
	public float NpcBounceValue = 50f;
	
	[Export]
	public float NpcBounceTime = 1f;
	
	public override void _Ready()
	{
		GetNode<CoffeeQueueManager>( "../CoffeeQueueManager" ).OnQueueAdvance += SpawnDoorOnPlayer;
	}
	
	private void OnBodyEntered( Node2D body )
	{
		if( body is not NpcCoffee npc ) return;
		
		Vector2 dir = ( npc.GlobalPosition - GlobalPosition ).Normalized();
		npc.AddForceImpulse( NpcBounceValue * dir, NpcBounceTime );
	}
	
	private void SpawnDoorOnPlayer()
	{
		var coffee_manager = GetNode<CoffeeQueueManager>( "../CoffeeQueueManager" );
		if( !coffee_manager.PlayerInQueue ) return;
		if( coffee_manager.PlayerQueueIndex != 0 ) return;
		
		GetNode<Node2D>( "../NextLevelDoor" ).GlobalPosition = this.GlobalPosition;
	}
	
	public void Highlight()
	{
		GetNode<Sprite2D>( "Sprite2D" ).Material.Set( "shader_parameter/width", 1 );
	}
	
	public void Unhighlight()
	{
		GetNode<Sprite2D>( "Sprite2D" ).Material.Set( "shader_parameter/width", 0 );
	}
}


