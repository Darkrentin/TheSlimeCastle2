using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : AllPlayer
{
	
	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -400.0f;
	public TileMap TileMap;
	public Node2D Target;
	
	public string Center;
	public string Up;
	public string Down;
	public string Left;
	public string Right;
	
	
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
		
		Debug.Text = StateMachine.CurrentState.Name;


		UpdateLocalCell();
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
			Multiplayer.GetUniqueId())
		{
			if(Center!=null && Center=="Spike")
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
		AniTr.Set("parameters/Crouch/blend_position", direction);
		
		
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

	public void UpdateLocalCell()
	{
		Vector2I FootPos = TileMap.LocalToMap(GlobalPosition+new Vector2(0,0));
		TileData Foot = TileMap.GetCellTileData(0, FootPos);
		Vector2I UnderFootPos = TileMap.LocalToMap(GlobalPosition+new Vector2(0,16));
		TileData UnderFoot = TileMap.GetCellTileData(0, UnderFootPos);
		Vector2I UpperFootPos = TileMap.LocalToMap(GlobalPosition+new Vector2(0,-16));
		TileData UpperFoot = TileMap.GetCellTileData(0, UpperFootPos);
		Vector2I LeftFootPos = TileMap.LocalToMap(GlobalPosition+new Vector2(-16,0));
		TileData LeftFoot = TileMap.GetCellTileData(0, LeftFootPos);
		Vector2I RightFootPos = TileMap.LocalToMap(GlobalPosition+new Vector2(16,0));
		TileData RightFoot = TileMap.GetCellTileData(0, RightFootPos);
		
		Target.GlobalPosition = new Vector2(8+RightFootPos.X*16,8+RightFootPos.Y*16);
		Center = "";
		Down = "";
		Up = "";
		Left = "";
		Right = "";
		if (Foot != null)
		{
			Center = Foot.GetCustomData("Type").ToString();
		}
		if (UnderFoot != null)
		{
			Down = UnderFoot.GetCustomData("Type").ToString();
		}
		if (UpperFoot != null)
		{
			Up = UpperFoot.GetCustomData("Type").ToString();
		}
		if (LeftFoot != null)
		{
			Left = LeftFoot.GetCustomData("Type").ToString();
		}
		if (RightFoot != null)
		{
			Right = RightFoot.GetCustomData("Type").ToString();
		}
		//GD.Print($"Center: {Center} Down: {Down} Up: {Up} Left: {Left} Right: {Right}");
	}

}

