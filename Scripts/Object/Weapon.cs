using Godot;
using System;

public partial class Weapon : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_body_entered(Node2D body)
	{
		if(body is Player currentPlayer)
		{
			int index = 0;
			foreach (Node2D spawn in GetTree().GetNodesInGroup("Spawn"))
			{
				if (int.Parse(spawn.Name) == index)
				{
					currentPlayer.GlobalPosition = spawn.GlobalPosition;
				}
				index++;
			}
		}
	}
}

