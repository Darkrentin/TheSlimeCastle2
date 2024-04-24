using Godot;
using System;

public partial class AllPlayer : CharacterBody2D
{
	public AnimatedSprite2D AniSpr;
	public CollisionShape2D Stand;
	public CollisionShape2D Crouch;
	public Sprite2D Sprite;
	public bool AnimationLock = false;
	public float direction;
	public Label Pseu;
	public AnimationTree AniTr;
	public AnimationPlayer Ani;
	public Node2D Face;
	public CharacterStateMachine StateMachine;

}
