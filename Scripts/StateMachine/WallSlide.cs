using Godot;
using System;

public partial class WallSlide : State
{
	[Export] public float JumpVelocity = -350.0f;
	[Export] private State AirState;
	[Export] private State StandState;
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
		(Player as Player).gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle()/4f;
		if (Player.IsOnFloor())
		{
			NextState = StandState;
			(Player as AllPlayer).CurAni = "Idle";
			PlayBack.Travel("Move");
			(Player as Player).gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
		}
		if((Player as Player).Right == "FLoor")
		{
			(Player as Player).Face.Scale = new Vector2(-1,1);
		}
		else if((Player as Player).Left == "FLoor")
		{
			(Player as Player).Face.Scale = new Vector2(1,1);
		}
		else
		{
			NextState = StandState;
			(Player as AllPlayer).CurAni = "Idle";
			PlayBack.Travel("Move");
			(Player as Player).gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
		}
	}
	public override void StateInput(InputEvent Event)
	{
		if (Event.IsActionPressed("Jump"))
		{
			jump();
		}
	}
	private void jump()
	{
		Player.Velocity= new Vector2(Player.Velocity.X,JumpVelocity);
		NextState = AirState;
		(Player as AllPlayer).CurAni = "JumpUp";
		PlayBack.Travel("JumpUp");
		(Player as Player).gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
		
	}
}
