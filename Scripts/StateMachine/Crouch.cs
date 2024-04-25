using Godot;
using System;

public partial class Crouch : State
{
	[Export] private State GroundState;
	[Export] State AirState;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(PlayBack==null)
		{
			PlayBack = (AnimationNodeStateMachinePlayback)(Player as AllPlayer).AniTr.Get("parameters/playback");
		}
	}
	public override void StateInput(InputEvent Event)
	{
		if(Event.IsActionReleased("Crouch"))
		{
			Stand();
		}
	}
	
	public override void StateProcess(double delta)
	{
		(Player as Player).Speed = 75.0f;
		if (!Player.IsOnFloor())
		{
			NextState = AirState;
			(Player as Player).Speed = 200.0f;
			(Player as AllPlayer).CurAni = "JumpDown";
			PlayBack.Travel("JumpDown");
		}
		else
		{
			(Player as AllPlayer).CurAni = "Crouch";
		}
	}
	
	public void Stand()
	{
		NextState = GroundState;
		(Player as Player).Speed = 200.0f;
		(Player as AllPlayer).CurAni = "Idle";
		PlayBack.Travel("Move");
	}
}
