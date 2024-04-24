using Godot;
using System;

public partial class Ground : State
{
	[Export] public float JumpVelocity = -350.0f;
	[Export] State AirState;
	[Export] State AttackState;
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
	}
	public override void StateProcess(double delta)
	{
		if (!Player.IsOnFloor())
		{
			NextState = AirState;
			NameState = "Air";
			PlayBack.Travel("JumpDown");
		}
	}

	private void jump()
	{
		Player.Velocity= new Vector2(Player.Velocity.X,JumpVelocity);
		NextState = AirState;
		NameState = "Air";
		PlayBack.Travel("JumpUp");
		
	}

	private void attack()
	{
		NextState = AttackState;
		NameState = "Attack";
		PlayBack.Travel("Atk1");
	}
}
