using Godot;
using System;
using static CharacterStates;

public partial class NPCSalad : NPCTopdown, IAttacker
{
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
				_targetSalad = Salad.GetClosestSalad( GlobalPosition );
				
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
	
	public void Shoot()
	{
		var direction = ( _player.GlobalPosition - GlobalPosition ).Normalized();
		Salad?.Throw( direction );
	}
}
