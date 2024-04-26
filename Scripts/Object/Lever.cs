using Godot;
using System;

public partial class Lever : Node2D
{
	[Export] Gate LinkedDoor;
	private bool In = false;
	[Export] private bool Active = false;
	private AnimationPlayer Ani;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Ani = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(In && Input.IsActionJustPressed("Interaction"))
		{
			if(!Active)
			{
				Active = true;
				Ani.Play("ON");
				LinkedDoor.OpenGate = true;
			}
			else
			{
				Active = false;
				Ani.Play("OFF");
				LinkedDoor.OpenGate = false;
			}
		}
	}
	
	private void _on_area_2d_body_entered(Node2D body)
	{
		if(body is Player) In = true;
	}


	private void _on_area_2d_body_exited(Node2D body)
	{
		if(body is Player) In = false;
	}
}



