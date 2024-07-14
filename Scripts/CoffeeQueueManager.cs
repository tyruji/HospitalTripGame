using Godot;
using System;

public partial class CoffeeQueueManager : Node
{
	[Export]
	private Timer _timer = null;

	[Export]
	private PackedScene _NpcPrefab = null;

	public int[] QueueSpotTargetCount = new int[ 0 ];

	public bool PlayerInQueue => PlayerQueueIndex >= 0;

	public int PlayerQueueIndex { get; set; } = -1;
	
	public Vector2 QueueLeavePosition => _queueLeavePoint.GlobalPosition;

	public event Action OnQueueAdvance;
	
	private Marker2D _spawnPoint = null;
	
	private Marker2D _queueLeavePoint = null;

	public override void _Ready()
	{
		_queueLeavePoint = GetNode<Marker2D>( "QueueLeavePoint" );
		_spawnPoint = GetNode<Marker2D>( "SpawnPoint" );
		_timer = GetNode<Timer>( "Timer" );

		_timer.Timeout += AdvanceQueue;
		QueueSpotTargetCount = new int[ GetNode( "QueueSpots" ).GetChildCount() ];
	}

	public void AdvanceQueue()
	{
		OnQueueAdvance?.Invoke();
	}
	
	public void SpawnNewNpc()
	{
		var npc = ( Node2D ) _NpcPrefab.Instantiate();
		npc.GlobalPosition = _spawnPoint.GlobalPosition;
		GetNode( "../NPCs" ).AddChild( npc );
	}
}
