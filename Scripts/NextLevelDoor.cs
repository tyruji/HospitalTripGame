using Godot;
using System;

public partial class NextLevelDoor : Node2D
{
	[Export]
	protected PackedScene _NextScene = null;
	
	private static bool _IN_TRANSITION = false;
	
	protected PlayerBase _player = null;
	
	public override void _Ready()
	{
		SceneStart();
		_player = ( PlayerBase ) GetTree().GetFirstNodeInGroup( "player" );
	}
	
	protected void SceneStart()
	{
		this.Visible = true;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty( GetNode( "ScreenDarken" ), "modulate", new Color( 0,0,0,0 ), 1.0f ).SetTrans( Tween.TransitionType.Sine );
	}
	
	protected virtual void NextScene()
	{
		_player.Speed = 0;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty( GetNode( "ScreenDarken" ), "modulate", Colors.White, 1.0f ).SetTrans( Tween.TransitionType.Sine );
		tween.TweenCallback( Callable.From( LoadScene ) );
		tween.TweenCallback( Callable.From( LeaveTransition ) );
	}
	
	protected virtual void LoadScene()
	{
		if( _NextScene == null ) return;
		
		GetTree().ChangeSceneToPacked( _NextScene );
	} 
	
	protected void LeaveTransition() => _IN_TRANSITION = false;
	
	private void OnBodyEntered( Node2D body )
	{
		if( body is not PlayerBase || _IN_TRANSITION ) return;
		_IN_TRANSITION = true;
		NextScene();
	}	
}



