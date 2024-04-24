using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : AllPlayer
{
	
	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -400.0f;
	
	
	
	private Label Debug;
	
	public override void _Ready()
	{
		Ani = GetNode<AnimationPlayer>("AnimationPlayer");
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
		AniTr = GetNode<AnimationTree>("AnimationTree");
		AniTr.Active = true;
		Face = GetNode<Node2D>("S");
		StateMachine = GetNode<CharacterStateMachine>("CharacterStateMachine");
		
		Debug = GetNode<Label>("debug");
	}
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{	
		Debug.Text = StateMachine.CurrentState.Name;
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
			Multiplayer.GetUniqueId())
		{
			Vector2 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
			{
				velocity.Y += gravity * (float)delta;
			}	
			direction = Input.GetAxis("Left", "Right");
			if (direction != 0 && StateMachine.CheckIfCanMove())
			{
				velocity.X = direction * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			} 

			Velocity = velocity;
		}
		UpdateAnimation();
		MoveAndSlide();
		if (StateMachine.CurrentState.CanMove)
		{
			UpdateFacingDir();
		}

	}
	
	private void UpdateAnimation()
	{
		AniTr.Set("parameters/Move/blend_position", direction);
		
		
	}
	private void UpdateFacingDir()
	{
		if(direction<0)
		{
			Face.Scale = new Vector2(-1,1);
		}
		else if(direction>0)
		{
			Face.Scale = new Vector2(1,1);
		}
	}

}

