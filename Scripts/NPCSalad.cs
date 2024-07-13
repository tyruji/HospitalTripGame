using Godot;
using System;

public partial class NPCSalad : NPCTopdown, IAttacker
{
	public Salad Salad { get; set; }
	
	public bool CanAttack { get; set; } = true;
	
	public bool AttackAtEnd { get; set; } = true;
	
	public bool Attacking { get; set; }

	public bool HoldingGun { get; set; }

	public Vector2 AttackDirection { get; set; }
	
	private Salad _targetSalad = null;
	
	private PlayerSalad _player = null;
	
	public override void _Ready()
	{
		base._Ready();
		_player = ( PlayerSalad ) GetTree().GetFirstNodeInGroup( "player" );
	}
	
	public override void _Process( double delta )
	{
			// Najgorszy kod jaki napisalem w tym jamie
		if( Salad == null )
		{
			if( _targetSalad == null || _targetSalad.HasOwner )
				_targetSalad = Salad.GetClosestSalad( GlobalPosition );
				
			if( _targetSalad != null ) TargetPosition = _targetSalad.GlobalPosition;
			else TargetPosition = GlobalPosition;
		}
		else
		{
			Shoot();
		}
		
		base._Process( delta );
	}
	
	public void Shoot()
	{
		var direction = ( _player.GlobalPosition - GlobalPosition ).Normalized();
		Salad?.Throw( direction );
	}
}
