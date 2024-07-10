using Godot;
using System;

public partial class CoffeeQueueManager : Node
{
	  [Export]
	  public Node2D[] QueuePositions = new Node2D[ 0 ];

	  [Export]
	  private Timer _timer = null;

	  public bool PlayerInQueue => PlayerQueueIndex >= 0;

	  public int PlayerQueueIndex { get; set; } = -1;
	
	  public Vector2 QueueLeavePosition => _queueLeavePoint.GlobalPosition;

	  public event Action OnQueueAdvance;
	
	  private Marker2D _queueLeavePoint = null;

	  public override void _Ready()
	  {
			_queueLeavePoint = GetNode<Marker2D>( "QueueLeavePoint" );
			_timer = GetNode<Timer>( "Timer" );

			_timer.Timeout += AdvanceQueue;
	  }

	  public void AdvanceQueue()
	  {

			OnQueueAdvance?.Invoke();
	  }
}
