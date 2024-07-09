using Godot;
using System;

public partial class CoffeeQueueManager : Node
{
	  [Export]
	  public Node2D[] QueuePositions = new();

	  [Export]
	  private Timer _timer = null;

	  public int PlayerQueueIndex { get; private set; }

	  public event Action OnQueueAdvance;

	  public override void _Ready()
	  {
			_timer = GetNode<Timer>( "Timer" );

			_timer.Timeout.Connect( AdvanceQueue() );
	  }

	  public void AdvanceQueue()
	  {

			OnQueueAdvance?.Invoke();
	  }
}
