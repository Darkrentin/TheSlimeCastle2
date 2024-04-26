using Godot;
using System;

public partial class Gate : Node2D
{
	private bool Open = false;
	private int In = 0;
	[Export] public bool OpenGate = false;
	private AnimationPlayer Ani;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Ani = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Open && !OpenGate)
		{
			Ani.Play("Close");
			Open = false;
		}
		if(!Open && OpenGate)
		{
			Ani.Play("Open");
			Open = true;
		}
	}
}

