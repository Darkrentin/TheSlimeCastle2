using Godot;
using System;

public abstract partial class State : Node
{
	[Export] public bool CanMove = true; 
	public AnimationNodeStateMachinePlayback PlayBack;
	public State NextState;
	[Export] public string NameState;
	
	public CharacterBody2D Player;

	public virtual void StateInput(InputEvent Event)
	{
		
	}

	public virtual void StateProcess(double delta)
	{
		
	}

	public void Enter()
	{
		
	}

	public void Exit()
	{
		
	}
}
