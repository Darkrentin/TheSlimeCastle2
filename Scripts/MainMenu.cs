using Godot;
using System;
using Multi.Scripts;
using System.Collections.Generic;

public partial class MainMenu : Control
{
	[Export] public int port = 1234;
	private string ip = "127.0.0.1";
	public int MaxPlayers = 4;
	private ENetMultiplayerPeer peer;
	private LineEdit Ipt;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
		Ipt = GetNode<LineEdit>("IP");
		Ipt.Text = ip;
	}

	private void ConnectionFailed()
	{
		GD.Print("Connection Failed!");
	}

	private void ConnectedToServer()
	{
		GD.Print("Connected to Server!");
		RpcId(1,"sendPlayerInformation", GetNode<LineEdit>("Pseudo").Text,Multiplayer.GetUniqueId());
	}

	private void PeerDisconnected(long id)
	{
		GD.Print("Peer Disconnected!" + id.ToString());
	}

	private void PeerConnected(long id)
	{
		GD.Print("Peer Connected!" + id.ToString());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_host_pressed()
	{
		peer = new ENetMultiplayerPeer();
		var error = peer.CreateServer(port, MaxPlayers);
		if (error!= Error.Ok)
		{
			GD.Print("Error creating server: " + error.ToString());
			return;
		}
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Server Created!");
		sendPlayerInformation(GetNode<LineEdit>("Pseudo").Text,1);
		
		var scene = ResourceLoader.Load<PackedScene>("res://Scenes/Menu/Lobby.tscn").Instantiate<Control>();
		GetTree().Root.AddChild(scene);
		this.Hide();
		
	}


	private void _on_join_pressed()
	{
		peer = new ENetMultiplayerPeer();
		peer.CreateClient(Ipt.Text, port);
		
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Client Created!");
		var scene = ResourceLoader.Load<PackedScene>("res://Scenes/Menu/Lobby.tscn").Instantiate<Control>();
		GetTree().Root.AddChild(scene);
		scene.GetNode<Button>("Start").Hide();
		this.Hide();
	}

	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void sendPlayerInformation(string name, int id)
	{
		//GD.Print($"Info Rec {name} {id}");
		PlayerInfo playerInfo = new PlayerInfo()
		{
			Name = name,
			Id = id
		};
		if (NotIn(GameManager.Players,playerInfo))
		{
			GameManager.Players.Add(playerInfo);
		}
		
		string res = "";
		foreach (var P in GameManager.Players)
		{
			res+=$" N:{P.Name} ID:{P.Id}";
		}
		GD.Print(res);

		if (Multiplayer.IsServer())
		{
			foreach (var P in GameManager.Players)
			{
				Rpc("sendPlayerInformation", P.Name, P.Id);
			}
		}
	}
	private bool NotIn(List<PlayerInfo> List, PlayerInfo Play)
	{
		bool res = true;
		foreach (var P in GameManager.Players)
		{
			if(P.Id==Play.Id)
			{
				res = false;
			}
		}
		return res;
	}
}

