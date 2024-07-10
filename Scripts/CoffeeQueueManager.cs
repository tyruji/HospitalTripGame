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

	  public event Action OnQueueAdvance;

	  public override void _Ready()
	  {
			_timer = GetNode<Timer>( "Timer" );

			_timer.Timeout += AdvanceQueue;
	  }

	  public void AdvanceQueue()
	  {

			OnQueueAdvance?.Invoke();
	  }
}
