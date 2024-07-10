using Godot;
using System;

public partial class NpcCoffee : NPCTopdown
{
	[Export]
	public float PlayerBounceValue = 50f;
	
	[Export]
	public float MinSpotCampingDistance = 100f;
	
	public bool InQueue { get; set; }
	
	[Export]
	public int TargetQueueIndex { get; set; } = -1;
	
	private Area2D _area = null;
	
	private CoffeeQueueManager _queueManager = null;
	
	private QueueSpot _targetSpot = null;
	
	private bool _gotCoffee = false;
	
	public override void _Ready()
	{
		base._Ready();
		_queueManager = GetNode<CoffeeQueueManager>( "../CoffeeQueueManager" );
		_area = GetNode<Area2D>( "Area2D" );
		
		_queueManager.OnQueueAdvance += UpdateTargetQueueSpot;
		_queueManager.OnQueueAdvance += FollowQueueSpot;
	}
	
	public override void _Process( double delta )
	{
		if( !_gotCoffee )
		{
			FollowQueueSpot();
			if( _targetSpot == null || ( !InQueue && _targetSpot.Taken
					&& GlobalPosition.DistanceSquaredTo( TargetPosition )
					< MinSpotCampingDistance * MinSpotCampingDistance ) )
			{
				TargetPosition = GlobalPosition;
			}
		}
		
		base._Process( delta );
	}
	
	private void OnBodyEntered( Node2D body )
	{
		if( !InQueue || body is not PlayerTopdown player ) return;
		
		var dir = ( player.GlobalPosition - GlobalPosition ).Normalized();
		
		GD.Print( "BOUNCE PLAYER" );
			// Ta funkcje musisz jeszcze napisac w kodzie gracza
		//player.AddForce( PlayerBounceValue * dir );
	}
	
	private void UpdateTargetQueueSpot()
	{
		if( InQueue && TargetQueueIndex == 0 )
		{
				// Get a coffee and leave the queue.
			TargetPosition = _queueManager.QueueLeavePosition;
			_gotCoffee = true;
			
			return;
		}
		
		if( InQueue )
		{
			_targetSpot = GetSpot( --TargetQueueIndex );
			return;
		}
		
		if( _queueManager.PlayerInQueue )
		{
			TargetQueueIndex = _queueManager.PlayerQueueIndex;
			_targetSpot = GetSpot( TargetQueueIndex - 1 );
			return;
		}
		
		foreach( var queue_spot in _queueManager.GetNode( "QueueSpots" ).GetChildren() )
		{
			if( queue_spot is not QueueSpot spot || spot.Taken ) continue;
			
			TargetQueueIndex = spot.QueueIndex;
			break;
		}
		
		_targetSpot = GetSpot( TargetQueueIndex );
	}
	
	private void FollowQueueSpot()
	{
		if( _targetSpot == null || _gotCoffee ) return;
		TargetPosition = _targetSpot.GlobalPosition;
	}
	
	private QueueSpot GetSpot( int index ) => ( QueueSpot )_queueManager.GetNode( "QueueSpots" ).GetChild( index );
}



