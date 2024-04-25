using Godot;
using System;

public partial class Attack : State
{
	[Export] State ReturnState;
	private Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(PlayBack==null)
		{
			PlayBack = (AnimationNodeStateMachinePlayback)(Player as AllPlayer).AniTr.Get("parameters/playback");
		}
	}
	public override void StateInput(InputEvent Event)
	{
		if (Event.IsActionPressed("Atk1"))
		{
			timer.Start();
		}
	}
	
	private void _on_animation_tree_animation_finished(StringName anim_name)
	{
		if(anim_name=="Atk1")
		{
			if (timer.IsStopped())
			{
				NextState = ReturnState;
				(Player as AllPlayer).CurAni = "Idle";
				PlayBack.Travel("Move");
			}
			else
			{
				(Player as AllPlayer).CurAni = "Atk2";
				PlayBack.Travel("Atk2");
			}
		}
		if(anim_name=="Atk2")
		{
			NextState = ReturnState;
			(Player as AllPlayer).CurAni = "Idle";
			PlayBack.Travel("Move");
		}
	}
	
}

