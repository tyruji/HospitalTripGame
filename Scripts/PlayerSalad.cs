using Godot;
using System;
using static CharacterStates;

public partial class PlayerSalad : PlayerTopdown
{
	public Salad Salad { get; set; }
	
	private Sprite2D _normalSprite = null;
	
	private Sprite2D _saladSprite = null;
	
	public override void _Ready()
	{
		base._Ready();
		_normalSprite = GetNode<Sprite2D>( "Sprite2D" );
		_saladSprite = GetNode<Sprite2D>( "saladSprite" );
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
	}
	
	public override void Shoot()
	{
		Salad?.Throw( AttackDirection );
	}
}
