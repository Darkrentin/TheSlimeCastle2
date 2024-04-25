using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class OtherPlayer : AllPlayer
{

	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -400.0f;
	public override void _Ready()
	{
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
		Face = GetNode<Node2D>("S");
		Ani = GetNode<AnimationPlayer>("AnimationPlayer");
		
	}

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		if (Ani.CurrentAnimation != CurAni && (CurAni!="Idle" && CurAni!="Crouch"))
		{
			Ani.Play(CurAni);
		}
		if (CurAni=="Idle")
		{
			if (direction==0 && Ani.CurrentAnimation != "Idle")
			{
				Ani.Play("Idle");
			}
			else if (direction!=0 && Ani.CurrentAnimation != "Run")
			{
				Ani.Play("Run");
			}
		}
		if (CurAni=="Crouch")
		{
			if (direction==0 && Ani.CurrentAnimation != "CrouchIdle")
			{
				Ani.Play("CrouchIdle");
			}
			else if (direction!=0 && Ani.CurrentAnimation != "CrouchWalk")
			{
				Ani.Play("CrouchWalk");
			}
		}
		MoveAndSlide();
		UpdateFacingDir();
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

