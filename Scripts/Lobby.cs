using Godot;
using System;

public partial class Lobby : Control
{
	private Control MainMenu;
	private Control PlayerList;
	private Label ErrorMsg;
	private Button BackButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MainMenu = GetParent().GetNode<Control>("Menu");
		PlayerList = GetNode<Control>("Player");
		ErrorMsg = GetNode<Label>("Error");
		BackButton = GetNode<Button>("Back");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		UpdateList();
		if(GameManager.Players.Count==0)
		{
			PlayerList.Visible = false;
			BackButton.Visible = true;
			ErrorMsg.Text = "ERROR: NO PARTY FOUND";
			//(MainMenu as MainMenu).DisplayError();
			//MainMenu.Visible = true;
			//QueueFree();
		}
		else
		{
			PlayerList.Visible = true;
			BackButton.Visible = false;
			ErrorMsg.Text = "";
		}
	}
	
	private void _on_back_pressed()
	{
		(MainMenu as MainMenu).peer.DisconnectPeer(Multiplayer.GetUniqueId());
		(MainMenu as MainMenu).DisplayError();
		MainMenu.Visible = true;
		QueueFree();
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

