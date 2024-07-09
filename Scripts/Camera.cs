using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export]
	public Node2D Target { get; set; } = null;
	
	[Export]
	public float Dampening = 10.0f;

	public override void _Process(double delta)
	{
		if( Target == null ) return;
		
		GlobalPosition = GlobalPosition.Lerp( Target.GlobalPosition, ( float ) delta * Dampening );
	}
}
