using Godot;
using System;

public partial class Death : State
{
	[Export] State StandState;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(PlayBack==null)
		{
			PlayBack = (AnimationNodeStateMachinePlayback)(Player as AllPlayer).AniTr.Get("parameters/playback");
		}
	}
	private void _on_animation_tree_animation_finished(StringName anim_name)
	{
		if(anim_name == "Death")
		{
			NextState = StandState;
			(Player as AllPlayer).CurAni = "Move";
			PlayBack.Travel("Move");
			GoDeath();
		}
	}
	public void GoDeath()
	{
		int index = 0;
		foreach (Node2D spawn in GetTree().GetNodesInGroup("Spawn"))
		{
			if (int.Parse(spawn.Name) == index)
			{
				Player.GlobalPosition = spawn.GlobalPosition;
			}
			index++;
		}
	}
	
}

