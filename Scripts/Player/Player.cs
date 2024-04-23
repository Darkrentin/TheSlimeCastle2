using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -400.0f;
	
	private AnimatedSprite2D AniSpr;
	private bool AnimationLock = false;
	private Vector2 direction;
	private Label Pseu;
	
	public override void _Ready()
	{
		AniSpr = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
		//SetMultiplayerAuthority(int.Parse(Name));
	}
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
			Multiplayer.GetUniqueId())
		{
			Vector2 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
			{
				velocity.Y += gravity * (float)delta;
				Jump(velocity);
			}	
			else
			{
				AnimationLock = false;
			}
			// Handle Jump.
			if (true)
			{
				if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
					velocity.Y = JumpVelocity;

				direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
				if (direction != Vector2.Zero)
				{
					velocity.X = direction.X * Speed;
				}
				else
				{
					velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				}

				Velocity = velocity;
			}
			UpdateAnimation();
			MoveAndSlide();
			UpdateFacingDir();
		}
		
	}
	
	private void UpdateAnimation()
	{
		if(!AnimationLock)
		{
			if(direction.X!=0)
			{
				AniSpr.Play("Run");
			}
			else
			{
				AniSpr.Play("Idle");
			}
		}
	}
	private void UpdateFacingDir()
	{
		if(direction.X>0)
		{
			AniSpr.FlipH = false;
		}
		else if(direction.X<0)
		{
			AniSpr.FlipH = true;
		}
	}
	
	private void Jump(Vector2 velocity)
	{
		if(velocity.Y<0)
		{
			AniSpr.Play("JumpUp");
		}
		else if(velocity.Y==0)
		{
			AniSpr.Play("Idle");
		}
		else
		{
			AniSpr.Play("JumpDown");
		}
		AnimationLock = true;
	}
}
