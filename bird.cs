using Godot;
using System;

public partial class bird : Area2D
{
	
	// To add bird settings here


	// For movement related
	private Vector3 acceleration;
	private Vector2 velocity;
	private Vector2 forward;


	// To use to update the bird's position
	private Transform2D cachedTransform;
	
	// For following a certain target
	private Transform2D target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cachedTransform = Transform;
		forward = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
