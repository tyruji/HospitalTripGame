using Godot;
using System;

public partial class QueueSpot : Sprite2D
{
	public int QueueIndex { get; private set; } = 0;
	
	public bool Taken { get; private set; } = false;
	
	public Node2D QueueOccupator { get; private set; }
	
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
		if( body is PlayerCoffee player )
		{
			_queueManager.PlayerQueueIndex = QueueIndex;
			Taken = true;
			QueueOccupator = player;
			player.Highlight();
			return;
		}
		
		if( body is not NpcCoffee npc ) return;
		
		QueueOccupator = npc;
		npc.InQueue = true;
		Taken = true;
		npc.Highlight();
	}

	private void OnBodyExit( Node2D body )
	{
		if( body != QueueOccupator ) return;
		
		QueueOccupator = null;
		
		Taken = false;
		
		if( body is PlayerCoffee player )
		{
			_queueManager.PlayerQueueIndex = -1;
			
			player.Unhighlight();
			return;
		}
		
		if( body is not NpcCoffee npc ) return;
		
		npc.InQueue = false;
		npc.Unhighlight();
	}
}
