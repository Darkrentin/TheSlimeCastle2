using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : AllPlayer
{
	
	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -400.0f;
	public TileMap TileMap;
	public Node2D Target;
	
	
	
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
		TileMap = (TileMap)GetTree().GetNodesInGroup("LvlTest")[0];
		Target = GetNode<Node2D>("Target");
	}
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2I FootPos = TileMap.LocalToMap(GlobalPosition+new Vector2(0,0));
		TileData Foot = TileMap.GetCellTileData(0, FootPos);
		Vector2I UnderFootPos = TileMap.LocalToMap(GlobalPosition+new Vector2(0,16));
		TileData UnderFoot = TileMap.GetCellTileData(0, UnderFootPos);
		Vector2I UpperFootPos = TileMap.LocalToMap(GlobalPosition+new Vector2(0,-16));
		TileData UpperFoot = TileMap.GetCellTileData(0, UpperFootPos);
		
		string res = "";
		if(Foot!=null)
		{
			res += $" Here: {Foot.GetCustomData("Type").ToString()}";
		}
		if(UnderFoot!=null)
		{
			res += $" Under: {UnderFoot.GetCustomData("Type").ToString()}";
		}
		if(UpperFoot!=null)
		{
			res += $" Under: {UpperFoot.GetCustomData("Type").ToString()}";
		}
		GD.Print(res);
		Target.GlobalPosition = new Vector2(8+FootPos.X*16,8+FootPos.Y*16);
		Debug.Text = StateMachine.CurrentState.Name;
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
			Multiplayer.GetUniqueId())
		{
			if(Foot!=null && (string)Foot.GetCustomData("Type")=="Spike")
			{
				Death();
			}
			
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
	public void Death()
	{
		int index = 0;
		foreach (Node2D spawn in GetTree().GetNodesInGroup("Spawn"))
		{
			if (int.Parse(spawn.Name) == index)
			{
				GlobalPosition = spawn.GlobalPosition;
			}
			index++;
		}
	}

}

