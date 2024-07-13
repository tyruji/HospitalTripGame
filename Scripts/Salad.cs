using Godot;
using System;

public partial class Salad : Node2D
{
	[Export]
	private float _Speed = 1000f;
	
	[Export]
	private float _BounceValue = 1000f;
	
	[Export]
	private float _BounceTime = 1.0f;
	
	public bool HasOwner => _saladHolder != null;
	
	private bool _thrown = false;
	
	private Vector2 _direction = Vector2.Zero;
	
	private Node2D _saladHolder = null;
	
	private Area2D _area = null;
	
	private static readonly Godot.Collections.Array<Salad> _SALAD_ARRAY = new Godot.Collections.Array<Salad>();
	
	public static Salad GetClosestSalad( Vector2 pos )
	{
		Salad closest_salad = null;
		float last_distance = float.MaxValue;
		
		foreach( var salad in _SALAD_ARRAY )
		{
			float new_distance = salad.GlobalPosition.DistanceSquaredTo( pos );
			
			if( new_distance > last_distance || salad.HasOwner ) continue;
			
			closest_salad = salad;
			last_distance = new_distance;
		}
		
		return closest_salad;
	}
	
	public override void _Ready()
	{
		_SALAD_ARRAY.Add( this );
		_area = GetNode<Area2D>( "Area2D" );
	}
	
	public override void _Process( double delta )
	{
		if( !_thrown ) return;
		
		GlobalPosition += _Speed * ( float ) delta * _direction;
	}
	
	public void Throw( Vector2 dir )
	{
		_thrown = true;
		_direction = dir;
		GlobalPosition = _saladHolder.GlobalPosition;
		
		this.Visible = true;
		_area.CallDeferred( "set", "monitoring", true );
		
		if( _saladHolder is NPCSalad npc )
		{
			npc.Salad = null;
			npc.HoldingGun = false;
			return;
		}
		
		if( _saladHolder is PlayerSalad player )
		{
			player.Salad = null;
			player.HoldingGun = false;
			player.CanAttack = false;
			return;
		}
	}
	
	private void OnBodyEntered( Node2D body )
	{
		if( _thrown )
		{
			HandleThrown( body );
			return;
		}
		HandleOnGround( body );
	}
	
	private void HandleThrown( Node2D body )
	{
		if( body == _saladHolder ) return;
		
		if( body is NPCSalad npc )
		{
			npc.AddForceImpulse( _BounceValue * _direction, _BounceTime );
			_saladHolder = null;
			_thrown = false;
			return;
		}
		
		if( body is PlayerSalad player )
		{
			player.AddForceImpulse( _BounceValue * _direction, _BounceTime );
			_saladHolder = null;
			_thrown = false;
			return;
		}
		
		_saladHolder = null;
		_thrown = false;
	}
	
	private void HandleOnGround( Node2D body )
	{
		if( body is NPCSalad npc )
		{
			npc.Salad = this;
			npc.HoldingGun = true;
			_saladHolder = npc;
			this.Visible = false;
			_area.CallDeferred( "set", "monitoring", false );
			return;
		}
		
		if( body is PlayerSalad player )
		{
			player.CanAttack = true;
			player.Salad = this;
			player.HoldingGun = true;
			_saladHolder = player;
			this.Visible = false;
			_area.CallDeferred( "set", "monitoring", false );
			return;
		}
	}
}