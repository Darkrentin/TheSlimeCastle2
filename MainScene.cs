using Godot;
using System;

public partial class MainScene : Node2D
{
	[Export] private PackedScene playerScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int index = 0;
		foreach (var P in GameManager.Players)
		{
			Player currentPlayer = playerScene.Instantiate<Player>();
			currentPlayer.Name = P.Id.ToString();
			AddChild(currentPlayer);
			foreach (Node2D spawn in GetTree().GetNodesInGroup("Spawn"))
			{
				if (int.Parse(spawn.Name) == index)
				{
					currentPlayer.GlobalPosition = spawn.GlobalPosition;
				}
			}

			index++;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
