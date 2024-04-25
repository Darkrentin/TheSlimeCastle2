using Godot;
using System;

public partial class Stand : State
{
	[Export] public float JumpVelocity = -350.0f;
	[Export] State AirState;
	[Export] State AttackState;
	[Export] State CrouchState;
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
		if (Event.IsActionPressed("Jump"))
		{
			jump();
		}

		if (Event.IsActionPressed("Atk1"))
		{
			attack();
		}
		if(Event.IsActionPressed("Crouch"))
		{
			Crouch();
		}
	}
	public override void StateProcess(double delta)
	{
		if (!Player.IsOnFloor())
		{
			NextState = AirState;
			(Player as AllPlayer).CurAni = "JumpDown";
			PlayBack.Travel("JumpDown");
		}
		else
		{
			(Player as AllPlayer).CurAni = "Idle";
		}
	}

	private void jump()
	{
		Player.Velocity= new Vector2(Player.Velocity.X,JumpVelocity);
		NextState = AirState;
		(Player as AllPlayer).CurAni = "JumpUp";
		PlayBack.Travel("JumpUp");
		
	}

	private void attack()
	{
		NextState = AttackState;
		(Player as AllPlayer).CurAni = "Atk1";
		PlayBack.Travel("Atk1");
	}
	
	private void Crouch()
	{
		NextState = CrouchState;
		(Player as AllPlayer).CurAni = "Crouch";
		PlayBack.Travel("Crouch");
	}
}
