using Godot;
using System;

public partial class QueueSpot : Sprite2D
{
	public int QueueIndex { get; private set; } = 0;
	
	public bool Taken { get; private set; } = false;
	
	private CoffeeQueueManager _queueManager = null;
	
	public override void _Ready()
	{
		_queueManager = GetNode<CoffeeQueueManager>( "../../" );
		QueueIndex = GetParent().GetNode( this.Name.ToString() ).GetIndex();
		GD.Print( QueueIndex );
	}
	
	private void OnBodyEntered( Node2D body )
	{
		if( Taken ) return;
		
			// Replace with function body.
		if( body is PlayerTopdown player )
		{
			_queueManager.PlayerQueueIndex = QueueIndex;
			Taken = true;
			return;
		}
		
		if( body is not NpcCoffee npc ) return;
		
		npc.InQueue = true;
		Taken = true;
	}

}





