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
	private void _on_animation_tree_animation_finished(StringName anim_name)
	{
		if(anim_name=="Atk1")
		{
			NextState = ReturnState;
		}
		if(anim_name=="Atk2")
		{
			NextState = ReturnState;
			(Player as AllPlayer).CurAni = "Idle";
			PlayBack.Travel("Move");
		}
	}
	
}

