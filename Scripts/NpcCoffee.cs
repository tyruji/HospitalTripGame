using Godot;
using System;

public partial class NpcCoffee : NPCTopdown
{
	[Export]
	public float PlayerBounceValue = 50f;
	
	public bool InQueue { get; set; }
	
	public int TargetQueueIndex { get; set; } = -1;
	
	private Area2D _area = null;
	
	private CoffeeQueueManager _queueManager = null;
	
	private QueueSpot _targetSpot = null;
	
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
		if( InQueue || _targetSpot == null || _targetSpot.Taken  )
		{
			_allowMovement = false;
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
		if( InQueue )
		{
			_targetSpot = GetSpot( ++TargetQueueIndex );
			return;
		}
		
		if( _queueManager.PlayerInQueue )
		{
			TargetQueueIndex = _queueManager.PlayerQueueIndex;
			_targetSpot = GetSpot( TargetQueueIndex );
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
		if( _targetSpot == null ) return;
		TargetPosition = _targetSpot.GlobalPosition;
		_allowMovement = true;
	}
	
	private QueueSpot GetSpot( int index ) => ( QueueSpot )_queueManager.GetNode( "QueueSpots" ).GetChild( index );
}



