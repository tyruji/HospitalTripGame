using Godot;
using System;

public partial class QueueSpot : Sprite2D
{
	public int QueueIndex { get; private set; } = 0;
	
	public bool Taken { get; private set; } = false;
	
	private CoffeeQueueManager _queueManager = null;
	
	private Node2D _queueOccupator = null;
	
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
			_queueOccupator = player;
			return;
		}
		
		if( body is not NpcCoffee npc ) return;
		
		_queueOccupator = npc;
		npc.InQueue = true;
		Taken = true;
	}

	private void OnBodyExit( Node2D body )
	{
		if( body != _queueOccupator ) return;
		
		_queueOccupator = null;
		
		Taken = false;
		
		if( body is PlayerTopdown player )
		{
			_queueManager.PlayerQueueIndex = -1;
			
			return;
		}
		
		if( body is not NpcCoffee npc ) return;
		
		npc.InQueue = false;
	}
}
