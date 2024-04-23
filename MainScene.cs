using Godot;
using System;

public partial class MainScene : Node2D
{
	[Export] private PackedScene PlayerScene;
	[Export] private PackedScene OtherPlayerScene;
	private Camera2D Cam;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Cam = GetNode<Camera2D>("Camera2D");
		int index = 0;
		foreach (var P in GameManager.Players)
		{
			Player currentPlayer;
			if(P.Id==Multiplayer.GetUniqueId())
			{
				currentPlayer = PlayerScene.Instantiate<Player>();
				currentPlayer.GetNode<RemoteTransform2D>("RemoteTransform2D").RemotePath = Cam.GetPath();
			}
			else
			{
				currentPlayer = OtherPlayerScene.Instantiate<Player>();
				currentPlayer.GetNode<Label>("Pseudo").Text = P.Name;
			}
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
