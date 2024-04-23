using Godot;
using System;

public partial class Lobby : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateList();
	}
	private void _on_start_pressed()
	{
		Rpc("StartGame");
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true,TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void StartGame()
	{
		foreach (var P in GameManager.Players)
		{
			GD.Print(P.Name+ " Play!");
		}
		var scene = ResourceLoader.Load<PackedScene>("res://MainScene.tscn").Instantiate<Node2D>();
		GetTree().Root.AddChild(scene);
		this.Hide();
	}
	public void UpdateList()
	{
		int index = 0;
		foreach (Label l in GetTree().GetNodesInGroup("NameTag"))
		{
			if(index<GameManager.Players.Count)
			{
				l.Text = $"Player {index+1} : "+GameManager.Players[index].Name;
			}
			else
			{
				l.Text = $"Player {index+1} : ";
			}
			index++;
		}
	}
}
