using System;
using Godot;
using static CharacterStates;

public static class CharacterStates
{
	public static readonly NullState NULL_STATE = new NullState();
	public static readonly MovingState MOVING_STATE = new MovingState();
	public static readonly MovingGunState MOVING_GUN_STATE = new MovingGunState();
	public static readonly IdleState IDLE_STATE = new IdleState();
	public static readonly IdleGunState IDLE_GUN_STATE = new IdleGunState();
	public static readonly PunchingState PUNCHING_STATE = new PunchingState();
	public static readonly ShootingState SHOOTING_STATE = new ShootingState();
}

public class NullState : State { }

public abstract class CharacterState : State
{
	protected abstract StateAnimationNames AnimationNames { get; }
	protected virtual bool UsePreviousDirection => false;

	public override void Enter( IStateHolder stateHolder )
	{
		if( stateHolder is not IAnimatable animatable ) return;
		if( stateHolder is not IMoveable moveable ) return;

		var dir = UsePreviousDirection ? moveable.PreviousDirection : moveable.Direction;

		animatable.Sprite.FlipH = dir.X < 0;

		string anim_name = AnimationNames.side;

		if( dir.X > .33 || dir.X < -.33 )
		{
			anim_name = AnimationNames.side;
		}
		else if( dir.Y < 0 )
		{
			anim_name = AnimationNames.up;
		}
		else if( dir.Y > 0 )
		{
			anim_name = AnimationNames.down;
		}

		animatable.AnimationPlayer.Play( anim_name );
	}
}

public class MovingState : CharacterState
{
	protected override StateAnimationNames AnimationNames => new( "walk_side", "walk_up", "walk_down" );

	public override void Handle( IStateHolder stateHolder )
	{
		if( stateHolder is not IMoveable moveable )
		{
			GD.PrintErr( "No IMoveable interface on object" ); 
			return;
		}
		moveable.Velocity = moveable.Speed * moveable.Direction;

		if( stateHolder is IAttacker attacker )
		{
			if( attacker.Attacking )
			{
				Next( stateHolder, PUNCHING_STATE );
				return;
			}
			if( attacker.HoldingGun )
			{
				Next( stateHolder, MOVING_GUN_STATE );
				return;
			}
		}

		if( moveable.Direction.LengthSquared() <= 0 )
		{
			Next( stateHolder, IDLE_STATE );
			return;
		}

		if( moveable.Direction != moveable.PreviousDirection )
		{
			Next( stateHolder, MOVING_STATE );
		}
	}
}
public class MovingGunState : CharacterState
{
	protected override StateAnimationNames AnimationNames
		=> new( "walk_side_gun", "walk_up_gun", "walk_down_gun" );

	public override void Handle( IStateHolder stateHolder )
	{
		if( stateHolder is not IMoveable moveable )
		{
			GD.PrintErr( "No IMoveable interface on object" );
			return;
		}
		moveable.Velocity = moveable.Speed * moveable.Direction;

		if( stateHolder is not IAttacker attacker )
		{
			GD.PrintErr( "No IAttacker interface on attacking object" );
			return;
		}   

		if( attacker.Attacking )
		{
			Next( stateHolder, SHOOTING_STATE );
			return;
		}

		if( !attacker.HoldingGun )
		{
			Next( stateHolder, MOVING_STATE );
			return;
		}

		if( moveable.Direction.LengthSquared() <= 0 )
		{
			Next( stateHolder, IDLE_GUN_STATE );
			return;
		}

		if( moveable.Direction != moveable.PreviousDirection )
		{
			Next( stateHolder, MOVING_GUN_STATE );
		}
	}
}

public class IdleState : CharacterState
{
	protected override StateAnimationNames AnimationNames => new( "idle_side", "idle_up", "idle_down" );
	protected override bool UsePreviousDirection => true;

	public override void Enter( IStateHolder stateHolder )
	{
		base.Enter( stateHolder );
		if( stateHolder is not IMoveable moveable ) return;

		moveable.Velocity = Vector2.Zero;
	}

	public override void Handle( IStateHolder stateHolder )
	{
		if( stateHolder is not IMoveable moveable )
		{
			GD.PrintErr( "No IMoveable interface on object" );
			return;
		}
		
		moveable.Velocity = Vector2.Zero;

		if( stateHolder is IAttacker attacker )
		{
			if( attacker.Attacking )
			{
				Next( stateHolder, PUNCHING_STATE );
				return;
			}
			if( attacker.HoldingGun )
			{
				Next( stateHolder, IDLE_GUN_STATE );
				return;
			}
		}

		if( moveable.Direction.LengthSquared() > 0 )
		{
			Next( stateHolder, MOVING_STATE );
		}      
	}
}
public class IdleGunState : CharacterState
{
	protected override StateAnimationNames AnimationNames
		=> new( "idle_side_gun", "idle_up_gun", "idle_down_gun" );
	protected override bool UsePreviousDirection => true;

	public override void Enter( IStateHolder stateHolder )
	{
		base.Enter( stateHolder );
		if( stateHolder is not IMoveable moveable ) return;

		moveable.Velocity = Vector2.Zero;
	}

	public override void Handle( IStateHolder stateHolder )
	{
		if( stateHolder is not IMoveable moveable )
		{
			GD.PrintErr( "No IMoveable interface on object" );
			return;
		}

		if( stateHolder is not IAttacker attacker )
		{
			GD.PrintErr( "No IAttacker interface on attacking object" );
			return;
		}

		if( attacker.Attacking )
		{
			Next( stateHolder, SHOOTING_STATE );
			return;
		}

		if( !attacker.HoldingGun )
		{
			Next( stateHolder, IDLE_STATE );
			return;
		}

		if( moveable.Direction.LengthSquared() > 0 )
		{
			Next( stateHolder, MOVING_GUN_STATE );
		}
	}
}

public class PunchingState : CharacterState
{
	protected override StateAnimationNames AnimationNames
		=> new( "punch_side", "punch_up", "punch_down" );

	//protected override bool UsePreviousDirection => true;

	public override void Enter( IStateHolder stateHolder )
	{
		if( stateHolder is not IAnimatable animatable ) return;
		if( stateHolder is not IAttacker attacker ) return;

		// HERE MAKE THIS THE DIRECTION FROM THE PLAYER TO THE MOUSE POSITION
		var dir = attacker.AttackDirection;

		animatable.Sprite.FlipH = dir.X < 0;

		string anim_name = AnimationNames.side;

		if( dir.X > .33f || dir.X < -.33f )
		{
			anim_name = AnimationNames.side;
		}
		else if( dir.Y < 0 )
		{
			anim_name = AnimationNames.up;
		}
		else if( dir.Y > 0 )
		{
			anim_name = AnimationNames.down;
		}

		animatable.AnimationPlayer.Play( anim_name );
	}

	public override void Exit( IStateHolder stateHolder )
	{
		IAttacker attacker = ( IAttacker )stateHolder;

		attacker.Attacking = false;
	}

	public override void Handle( IStateHolder stateHolder )
	{
		if( stateHolder is not IAnimatable animatable )
		{
			Next( stateHolder, IDLE_STATE );
			return;
		}

		if( stateHolder is IMoveable moveable_p )
		{
			moveable_p.Velocity = moveable_p.Speed * moveable_p.Direction;
		}

		if( animatable.AnimationPlayer.IsPlaying() ) return;

		if( stateHolder is IMoveable moveable && moveable.Direction.LengthSquared() > 0 )
		{
			Next( stateHolder, MOVING_STATE );
			return;
		}

		Next( stateHolder, IDLE_STATE );
	}
}
public class ShootingState : CharacterState
{
	protected override StateAnimationNames AnimationNames
		=> new( "shoot_side", "shoot_up", "shoot_down" );

	//protected override bool UsePreviousDirection => true;

	public override void Enter( IStateHolder stateHolder )
	{
		if( stateHolder is not IAnimatable animatable ) return;
		if( stateHolder is not IAttacker attacker ) return;

		// HERE MAKE THIS THE DIRECTION FROM THE PLAYER TO THE MOUSE POSITION
		var dir = attacker.AttackDirection;

		animatable.Sprite.FlipH = dir.X < 0;

		string anim_name = AnimationNames.side;

		if( dir.X > .33f || dir.X < -.33f )
		{
			anim_name = AnimationNames.side;
		}
		else if( dir.Y < 0 )
		{
			anim_name = AnimationNames.up;
		}
		else if( dir.Y > 0 )
		{
			anim_name = AnimationNames.down;
		}

		animatable.AnimationPlayer.Play( anim_name );
	}

	public override void Exit( IStateHolder stateHolder )
	{
		IAttacker attacker = ( IAttacker )stateHolder;

		attacker.Attacking = false;
	}

	public override void Handle( IStateHolder stateHolder )
	{
		if( stateHolder is not IAnimatable animatable )
		{
			Next( stateHolder, IDLE_GUN_STATE );
			return;
		}
		if( stateHolder is IMoveable moveable_p )
		{
			moveable_p.Velocity = moveable_p.Speed * moveable_p.Direction;
		}

		if( animatable.AnimationPlayer.IsPlaying() ) return;

		if( stateHolder is IMoveable moveable && moveable.Direction.LengthSquared() > 0 )
		{
			Next( stateHolder, MOVING_GUN_STATE );
			return;
		}

		Next( stateHolder, IDLE_GUN_STATE );
	}
}

public struct StateAnimationNames
{
	public string side;
	public string up;
	public string down;

	public StateAnimationNames( string _side, string _up, string _down )
	{
		this.side = _side;
		this.up = _up;
		this.down = _down;
	}
}
