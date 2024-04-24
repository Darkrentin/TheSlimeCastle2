using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class CharacterStateMachine : Node
{
	public List<State> states = new List<State>();
	public State CurrentState;
	[Export] public string NameState;
	public CharacterBody2D Player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = GetParent<CharacterBody2D>();
		foreach (var Child in GetChildren())
		{
			if (Child is State)
			{
				states.Add(Child as State);
				(Child as State).Player = Player;
			}
		}
		CurrentState = states[0];
		
	}
	
	
	public bool CheckIfCanMove()
	{
		return CurrentState.CanMove;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		NameState = CurrentState.NameState;
		if(CurrentState.NextState!=null)
		{
			SwitchState(CurrentState.NextState);
		}
		CurrentState.StateProcess(delta);
	}
	private void SwitchState(State newState)
	{
		if(CurrentState!=null)
		{
			CurrentState.Exit();
			CurrentState.NextState = null;
		}

		CurrentState = newState;
		CurrentState.Enter();
	}

	public void _input(InputEvent Event)
	{
		CurrentState.StateInput(Event);
	}
}
