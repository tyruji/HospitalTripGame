using Godot;
using System;

public partial class NextLevelDoor : Node2D
{
	[Export]
	private PackedScene _NextScene = null;
	
	public override void _Ready()
	{
		SceneStart();
	}
	
	private void SceneStart()
	{
		this.Visible = true;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty( GetNode( "ScreenDarken" ), "modulate", new Color( 0,0,0,0 ), 1.0f ).SetTrans( Tween.TransitionType.Sine );
	}
	
	private void NextScene()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty( GetNode( "ScreenDarken" ), "modulate", Colors.White, 1.0f ).SetTrans( Tween.TransitionType.Sine );
		tween.TweenCallback( Callable.From( LoadScene ) );
	}
	
	private void LoadScene()
	{
		if( _NextScene == null ) return;
		
		GetTree().ChangeSceneToPacked( _NextScene );
	} 
	
	private void OnBodyEntered( Node2D body )
	{
		if( body is not PlayerBase ) return;
		NextScene();
	}	
}



