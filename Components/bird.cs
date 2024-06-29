using Godot;
using System;


/*
	The bird's behaviour is to stick around the player within a certain predetermined radius
	The bird should also avoid colliding to other birds that the player might own
	Also, while within radius, the bird will move within the defined area.

	TODO: 
		Targeting of enemy
		Firing of bullets
		Player input command's flock behaviour
*/
public partial class bird : Area2D
{
	
	// To add bird settings here


	// For movement related
	private Vector3 acceleration;
	private Vector2 velocity;
	private Vector2 forward;

	// For reference to the player's area
	private Area2D playerBirdLoiterArea;


	// To use to update the bird's position
	private Transform2D cachedTransform;
	
	// For following a certain target
	private Transform2D target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// We will find the player and notify that we belong to the player
		player playerNode = GetNode<player>("/root/Main/Player");

		// Ensure playerNode is not null
		if (playerNode != null)
		{
			// Adjust the path according to the actual hierarchy
			playerBirdLoiterArea = playerNode.GetNode<Area2D>("BirdLoiterArea"); // Use "ChildNode/BirdLoiterArea" if nested

			// Ensure playerArea2d is not null before using it
			if (playerBirdLoiterArea != null)
			{
				// Your code to work with playerArea2d
				playerBirdLoiterArea.Connect("area_entered", new Callable(this, nameof(OnPlayerAreaEntered)), (uint)0);
				playerBirdLoiterArea.Connect("area_exited", new Callable(this, nameof(OnPlayerAreaExited)), (uint)0);
			}
			else
			{
				GD.Print("BirdLoiterArea not found");
			}
		}
		else
		{
			GD.Print("playerNode is null");
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}


	public void OnPlayerAreaEntered(Area2D area)
	{
		if (area == this) // Check if the entered area is this bird
		{
			GD.Print("Bird has entered the player's area");
			// Implement your logic here
		}
	}

	public void OnPlayerAreaExited(Area2D area)
	{
		if (area == this) // Check if the exited area is this bird
		{
			GD.Print("Bird has exited the player's area");
			// Implement your logic here
		}
	}

}
