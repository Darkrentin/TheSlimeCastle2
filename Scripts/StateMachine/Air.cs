using Godot;
using System;

public partial class Air : State
{
	[Export] private State GroundState;
	[Export] private State WallSlide;
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

	public override void StateProcess(double delta)
	{
		if (Player.IsOnFloor())
		{
			NextState = GroundState;
			(Player as AllPlayer).CurAni = "Idle";
			PlayBack.Travel("Move");
		}
		else if(Player.Velocity.Y>=0 && ((Player as Player).Right == "FLoor" || (Player as Player).Left == "FLoor"))
		{
			NextState = WallSlide;
			(Player as AllPlayer).CurAni = "WallSlide";
			PlayBack.Travel("WallSlide");
		}
		else
		{
			if(Player.Velocity.Y<0)
			{
				(Player as AllPlayer).CurAni = "JumpUp";
				PlayBack.Travel("JumpUp");
			}
			else
			{
				(Player as AllPlayer).CurAni = "JumpDown";
				PlayBack.Travel("JumpDown");
			}
		}
	}
}
