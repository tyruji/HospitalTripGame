using Godot;
using System;

public partial class GuardNextLevelDoor : NextLevelDoor
{
	private PlayerTopdown _player = null;
	private Camera _cam = null;
	private Vector2 _startPlayerPos = Vector2.Zero;
	
	public override void _Ready()
	{
		base._Ready();
		_cam = ( Camera ) GetTree().GetFirstNodeInGroup( "camera" );
		_player = ( PlayerTopdown ) GetTree().GetFirstNodeInGroup( "player" );
		_startPlayerPos = _player.GlobalPosition;
	}
	
	protected override void NextScene()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenCallback( Callable.From( CameraToGuard ) );
		tween.TweenProperty( _cam, "zoom", Vector2.One * 2, 1.0f ).SetTrans( Tween.TransitionType.Sine );
		tween.TweenProperty( GetNode( "ScreenDarken" ), "modulate", Colors.White, 1.0f ).SetTrans( Tween.TransitionType.Sine );
		tween.TweenCallback( Callable.From( LoadScene ) );
		tween.TweenCallback( Callable.From( CameraToPlayer ) );
		tween.TweenCallback( Callable.From( SceneStart ) );
		tween.TweenProperty( _cam, "zoom", Vector2.One, 1.0f ).SetTrans( Tween.TransitionType.Sine );
		tween.TweenCallback( Callable.From( LeaveTransition ) );
	}
	
	private void CameraToGuard()
	{
		_cam.Target = ( Node2D ) GetParent().GetParent();
	}
	
	private void CameraToPlayer()
	{
		_cam.Target = _player;	
	}
	
	protected override void LoadScene()
	{
		_player.GlobalPosition = _startPlayerPos;
	}
}
