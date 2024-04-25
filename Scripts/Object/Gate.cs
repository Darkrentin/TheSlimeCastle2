using Godot;
using System;

public partial class Gate : Node2D
{
	private bool Open = false;
	private int In = 0;
	private AnimationPlayer Ani;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Ani = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Open && In==0)
		{
			Ani.Play("Close");
			Open = false;
		}
		if(!Open && In!=0)
		{
			Ani.Play("Open");
			Open = true;
		}
	}
	private void _on_area_2d_body_entered(Node2D body)
	{
		if(body is AllPlayer)
		{
			In++;
		}
	}


	private void _on_area_2d_body_exited(Node2D body)
	{
		if(body is AllPlayer)
		{
			In--;
		}
	}
}

