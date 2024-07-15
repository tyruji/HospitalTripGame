using Godot;
using System;

public partial class ChaseSpawner : Node2D
{
	[Export]
	private PackedScene _Npc = null;
	
	[Export]
	private int _MaxNpcs = 10;
	
	private int _npcCount = 0;
	
	public override void _Ready()
	{
		GetNode<Timer>( "Timer" ).Timeout += SpawnNpc;
	}
	
	private void SpawnNpc()
	{
		if( ++_npcCount > _MaxNpcs || _Npc == null ) return;
		
		var npc = ( Node2D ) _Npc.Instantiate();
		this.AddChild( npc );
		npc.GlobalPosition = this.GlobalPosition;
	}
}
