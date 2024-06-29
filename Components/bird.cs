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
	[Export]
	public float Speed;

	// For movement related
	private Vector3 acceleration;
	private Vector2 velocity;
	private Vector2 forward;

	// For reference to the player's area
	private Area2D playerBirdLoiterArea;
	private player birdOwner;


	// Bird States
	private BirdState birdState = BirdState.None;
	
	// For following a certain target
	private Transform2D target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// We will find the player and notify that we belong to the player
		birdOwner = GetNode<player>("/root/Main/Player");

		// Ensure playerNode is not null
		if (birdOwner != null)
		{
			// Adjust the path according to the actual hierarchy
			playerBirdLoiterArea = birdOwner.GetNode<Area2D>("BirdLoiterArea"); // Use "ChildNode/BirdLoiterArea" if nested

			// Ensure playerArea2d is not null before using it
			if (playerBirdLoiterArea != null)
			{
				// Your code to work with playerArea2d
				playerBirdLoiterArea.Connect("area_entered", new Callable(this, nameof(OnPlayerAreaEntered)), 0);
				playerBirdLoiterArea.Connect("area_exited", new Callable(this, nameof(OnPlayerAreaExited)), 0);

				birdState = BirdState.ApproachOwner;
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
	public override void _PhysicsProcess(double delta)
	{
		switch(birdState)
		{
			case BirdState.Idle:
				break;

			case BirdState.ApproachOwner :
				FollowOwner((float)delta);
				break;
			
			case BirdState.FollowOwner:
				FollowOwner((float)delta);
				break;

			case BirdState.Loiter:
				break;

			case BirdState.Attack:
				break;

			case BirdState.Defend:
				break;

			case BirdState.Dead:
				break;
		}
	}

#region Signals
	public void OnPlayerAreaEntered(Area2D area)
	{
		if (area == this) // Check if the entered area is this bird
		{
			if(birdState == BirdState.ApproachOwner)
			{
				// Notify that player has a new unit tagging along
				birdOwner.NotifyNewBird(this);
				birdOwner.Connect(nameof(birdOwner.PlayerIdle), new Callable(this, "OnPlayerIdle"), 0);
				birdOwner.Connect(nameof(birdOwner.PlayerRunning), new Callable(this, "OnPlayerRunning"),  0);
				birdOwner.Connect(nameof(birdOwner.PlayerDead), new Callable(this, "OnPlayerDead"), 0);

			}


			// We then can begin to loiter
			birdState = BirdState.Loiter;
			
		}
	}

	public void OnPlayerAreaExited(Area2D area)
	{
		if (area == this) // Check if the exited area is this bird
		{			
			if(birdState == BirdState.Loiter)
			{
				birdState = BirdState.FollowOwner;
			}
		}
	}

	private void OnPlayerIdle()
	{
		GD.Print("Player is idle");
	}
	
	private void OnPlayerRunning()
	{
		GD.Print("Player is moving");
	}

	private void OnPlayerDead()
	{
		GD.Print("Player is dead");
	}

#endregion

#region Private Manipulators
	private void FollowOwner(float delta)
	{
		if (birdOwner != null)
		{
			Vector2 direction = birdOwner.GlobalPosition - GlobalPosition;
			direction = direction.Normalized();
			velocity = direction * Speed;
			GlobalPosition += velocity * delta;
		}
	}


#endregion

}
