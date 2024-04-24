using Godot;
using System;

public partial class Air : State
{
	[Export] private State GroundState;
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
			NameState = "Ground";
			PlayBack.Travel("Move");
		}
		else
		{
			if(Player.Velocity.Y<0)
			{
				PlayBack.Travel("JumpUp");
			}
			else
			{
				PlayBack.Travel("JumpDown");
			}
		}
	}
}
