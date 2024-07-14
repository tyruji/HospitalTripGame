using Godot;
using System;

public partial class NpcCoffee : NPCTopdown
{
	[Export]
	public float PlayerBounceValue = 50f;
	
	[Export]
	public float PlayerBounceTime = 1f;
	
	[Export]
	public float NpcBounceValue = 50f;
	
	[Export]
	public float NpcBounceTime = 1f;
	
	[Export]
	public float MinSpotCampingDistance = 100f;
	
	public bool InQueue { get; set; }
	
	[Export]
	public int TargetQueueIndex { get; set; } = -1;
	
	private Area2D _area = null;
	
	private CoffeeQueueManager _queueManager = null;
	
	private QueueSpot _targetSpot = null;
	
	private bool _gotCoffee = false;
	
	private int _npcCount = 0;
	
	public override void _Ready()
	{
		base._Ready();
		_queueManager = GetNode<CoffeeQueueManager>( "../../CoffeeQueueManager" );
		_area = GetNode<Area2D>( "Area2D" );
		_npcCount = GetParent().GetChildCount();
		
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
			if( _targetSpot != null && !InQueue && _targetSpot.Taken ) UpdateTargetQueueSpot();
		}
		else if( GlobalPosition.DistanceSquaredTo( _queueManager.QueueLeavePosition )
					< MinPointDistance * MinPointDistance )
		{
			_queueManager.OnQueueAdvance -= UpdateTargetQueueSpot;
			_queueManager.OnQueueAdvance -= FollowQueueSpot;
			_queueManager.SpawnNewNpc();
			 CallDeferred( "queue_free" );
		}
		
		base._Process( delta );
	}
	
	private void OnBodyEntered( Node2D body )
	{
		Vector2 dir;
		
		if( body is NpcCoffee npc && !npc.InQueue )
		{
			dir = ( npc.GlobalPosition - GlobalPosition ).Normalized();
			npc.AddForceImpulse( NpcBounceValue * dir, NpcBounceTime );
		}
		
		if( body is not PlayerTopdown player )// || !InQueue )
			return;
		
		if( !InQueue && _queueManager.PlayerInQueue ) return;
		
		dir = ( player.GlobalPosition - GlobalPosition ).Normalized();
		
		player.AddForceImpulse( PlayerBounceValue * dir, PlayerBounceTime );
	}
	
	public void Highlight()
	{
		GetNode<Sprite2D>( "Sprite2D" ).Material.Set( "shader_parameter/width", 1 );
	}
	
	public void Unhighlight()
	{
		GetNode<Sprite2D>( "Sprite2D" ).Material.Set( "shader_parameter/width", 0 );
	}
	
	private void UpdateTargetQueueSpot()
	{
		if( _gotCoffee )
		{
			TargetPosition = _queueManager.QueueLeavePosition;
			return;
		}
		
		if( TargetQueueIndex >= 0 ) --_queueManager.QueueSpotTargetCount[ TargetQueueIndex ];
		
		if( InQueue && GetSpot( 0 ).QueueOccupator == this  )
		{
			TargetQueueIndex = -1;
			NpcBounceValue = 2 * PlayerBounceValue;
				// Get a coffee and leave the queue.
			TargetPosition = _queueManager.QueueLeavePosition;
			_gotCoffee = true;
			
			return;
		}
		
		if( InQueue )
		{
			_targetSpot = GetSpot( --TargetQueueIndex );
			
			++_queueManager.QueueSpotTargetCount[ TargetQueueIndex ];
			return;
		}
		
		//if( _queueManager.PlayerInQueue && _queueManager.PlayerQueueIndex > 0 )
		//{
			//TargetQueueIndex = _queueManager.PlayerQueueIndex;
			//_targetSpot = GetSpot( TargetQueueIndex - 1 );
			//++_queueManager.QueueSpotTargetCount[ TargetQueueIndex ];
			//return;
		//}
		
		foreach( var queue_spot in _queueManager.GetNode( "QueueSpots" ).GetChildren() )
		{
			if( queue_spot is not QueueSpot spot ) continue; // || spot.Taken ) continue;
			
			if( _queueManager.QueueSpotTargetCount[ spot.QueueIndex ] > _npcCount / SpotCount() )
				continue;
			
			TargetQueueIndex = spot.QueueIndex;
			break;
		}
		
		if( TargetQueueIndex < 0 || TargetQueueIndex >= SpotCount() )
		{
			return;
		}
		
		_targetSpot = GetSpot( TargetQueueIndex );
		++_queueManager.QueueSpotTargetCount[ TargetQueueIndex ];
	}
	
	private void FollowQueueSpot()
	{
		if( _targetSpot == null || _gotCoffee ) return;
		TargetPosition = _targetSpot.GlobalPosition;
	}
	
	private QueueSpot GetSpot( int index ) => ( QueueSpot )_queueManager.GetNode( "QueueSpots" ).GetChild( index );
	
	private int SpotCount() => _queueManager.GetNode( "QueueSpots" ).GetChildCount();
}



