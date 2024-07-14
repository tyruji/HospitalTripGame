using Godot;
using System;
using static CharacterStates;

public partial class NPCSalad : NPCTopdown, IAttacker
{
	[Export]
	public float PlayerBounceValue = 900f;
	
	[Export]
	public float PlayerBounceTime = 1f;
	
	[Export]
	public float NpcBounceValue = 500f;
	
	[Export]
	public float NpcBounceTime = 1f;
	
	public Salad Salad { get; set; }
	
	public bool CanAttack { get; set; } = false;
	
	public bool AttackAtEnd { get; set; } = true;
	
	public bool Attacking { get; set; }

	public bool HoldingGun { get; set; }

	public Vector2 AttackDirection { get; set; }
	
	private Salad _targetSalad = null;
	
	private PlayerSalad _player = null;
	
	private Sprite2D _normalSprite = null;
	
	private Sprite2D _saladSprite = null;
	
	public override void _Ready()
	{
		base._Ready();
		_normalSprite = GetNode<Sprite2D>( "Sprite2D" );
		_saladSprite = GetNode<Sprite2D>( "saladSprite" );
		_player = ( PlayerSalad ) GetTree().GetFirstNodeInGroup( "player" );
	}
	
	public override void _Process( double delta )
	{
		base._Process( delta );
		
		_saladSprite.FlipH = _normalSprite.FlipH;
		
		if( State == IDLE_GUN_STATE || State == SHOOTING_STATE || State == MOVING_GUN_STATE ) 
		{
			_normalSprite.Visible = false;
			_saladSprite.Visible = true;
		}
		else
		{
			_normalSprite.Visible = true;
			_saladSprite.Visible = false;
		}
		
			// Najgorszy kod jaki napisalem w tym jamie
		if( Salad == null && State != SHOOTING_STATE )
		{
			if( _targetSalad == null || _targetSalad.HasOwner )
			{
				if( GD.Randi() % 2 == 0 ) _targetSalad = Salad.GetRandomSalad();
				else _targetSalad = Salad.GetClosestSalad( GlobalPosition );
			}
				
				
			if( _targetSalad != null )
			{
				TargetPosition = _targetSalad.GlobalPosition;
				_targetSalad.TryPickup( this );
			}	
			else TargetPosition = GlobalPosition;
		}
		else
		{
			Attacking = true;
		}
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

		dir = ( player.GlobalPosition - GlobalPosition ).Normalized();
		
		player.AddForceImpulse( PlayerBounceValue * dir, PlayerBounceTime );
	}
	
	public void Shoot()
	{
		var direction = ( _player.GlobalPosition - GlobalPosition ).Normalized();
		Salad?.Throw( direction );
	}
}
