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
	
	private PlayerSalad _player = null;
	
	private float _pickupRadiusSqr = 0;
	
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
		
		_player = ( PlayerSalad )GetTree().GetFirstNodeInGroup( "player" );
		
		_pickupRadiusSqr = ( ( CircleShape2D ) 
			GetNode<CollisionShape2D>( "Area2D/CollisionShape2D" ).Shape ).Radius;
			
		_pickupRadiusSqr *= _pickupRadiusSqr;
	}
	
	public override void _Process( double delta )
	{
		if( !_thrown )
		{
			TryPickup( _player );
			return;
		}
		
		GlobalPosition += _Speed * ( float ) delta * _direction;
	}
	
	public void TryPickup( Node2D body )
	{
		if( _thrown || this.GlobalPosition.DistanceSquaredTo( body.GlobalPosition ) > _pickupRadiusSqr ) return;
		
		HandleOnGround( body );
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
			npc.CanAttack = false;
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
		
		_saladHolder = null;
		_thrown = false;
		
		if( body is NPCSalad npc )
		{
			npc.AddForceImpulse( _BounceValue * _direction, _BounceTime );
			return;
		}
		
		if( body is PlayerSalad player )
		{
			player.AddForceImpulse( _BounceValue * _direction, _BounceTime );
			return;
		}
	}
	
	private void HandleOnGround( Node2D body )
	{
		if( _saladHolder != null ) return;
		
		if( body is NPCSalad npc && npc.Salad == null )
		{
			_saladHolder = npc;
			npc.Salad = this;
			npc.HoldingGun = true;
			npc.CanAttack = true;
			this.Visible = false;
			_area.CallDeferred( "set", "monitoring", false );
			return;
		}
		
		if( body is PlayerSalad player && player.Salad == null)
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
