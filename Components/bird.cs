using Godot;
using System;
using System.Collections.Generic;


/*
	The bird's behaviour is to stick around the player within a certain predetermined radius
	The bird should also avoid colliding to other birds that the player might own
	Also, while within radius, the bird will move within the defined area.

	TODO: 
		Targeting of enemy
		Firing of bullets
		Player input command's flock behaviour
*/
public partial class bird : CharacterBody2D
{
	
	// To add bird settings here
	[Export]
	public float Speed;

	[Export]
	public float SocialDistancingFromPlayer;

	// For movement related
	private Vector3 acceleration;
	private Vector2 velocity;
	private Vector2 forward;

	// For reference to the player's area
	private player birdOwner;


	// Bird States
	private BirdState birdState = BirdState.None;

	//Animation
	private AnimatedSprite2D animationController;
	
	// For following a certain target
	private Transform2D target;


	// For bird flocking behaviour
	private static int totalNumberOfBirds = 0;
	private static List<bird> otherBirds = new List<bird>();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// We will find the player and notify that we belong to the player
		birdOwner = GetNode<player>("/root/Main/Player");
		animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Ensure playerNode is not null
		if (birdOwner != null)
		{
			// Notify that player has a new unit tagging along
			GD.Print("New bird joined the flock!");
			birdOwner.Connect(nameof(player.PlayerIdle), new Callable(this, "OnPlayerIdle"), 0);
			birdOwner.Connect(nameof(player.PlayerRunning), new Callable(this, "OnPlayerRunning"), 0);
			birdOwner.Connect(nameof(player.PlayerDead), new Callable(this, "OnPlayerDead"), 0);

			// Here we will get the number of birds within an area
			totalNumberOfBirds++;
			otherBirds.Add(this);

			birdState = BirdState.ApproachOwner;
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

	private void OnPlayerIdle()
	{
		// GD.Print("Player is idle");

		if(IsWithinDistanceOfPlayer())
		{
			birdState = BirdState.Idle;
		}
	}
	
	private void OnPlayerRunning()
	{
		// GD.Print("Player is moving");
		birdState = BirdState.FollowOwner;
	}

	private void OnPlayerDead()
	{
		GD.Print("Player is dead");
	}

#endregion

#region Private Manipulators

	private void GetBirdsInArea()
	{

	}


	private void FollowOwner(float delta)
	{
		if (birdOwner != null)
		{
			Vector2 direction = birdOwner.GlobalPosition - GlobalPosition;
			Velocity = direction.Normalized() * Speed;
	
			if(direction.X > 0)
			{
				animationController.FlipH = false;
			}
			else if(direction.X < 0)
			{
				animationController.FlipH = true;
			}
	
			if (IsWithinDistanceOfPlayer())
			{
				Velocity = Vector2.Zero; // Stop moving
				if(birdState == BirdState.FollowOwner)
				{
					animationController.Animation = "Idle";
					animationController.Play();
					birdState = BirdState.Idle;
				}
			}
			else
			{
				animationController.Animation = "Run";
				animationController.Play();
			}
	
			// Move using CharacterBody2D
			MoveAndSlide(); // Provide a second argument for the floor normal
			
			// Update the forward vector
			forward = direction;
		}
	}

	// This a mini boid behaviour
	private void BoidBehaviour()
	{
		/*
			There are 3 parts to this behaviour that will make it look more natural
			We will try to base it of a simple BOID system
			1) Speration: Avoid flocking over other Boids and player
			2) Alignment: Align moving towards the same direction
			3) Cohesion: Moving towrads the midsection of the crowd
		*/


	}

	private Vector2 SteerTowards(Vector2 steerDirection)
	{
		Vector2 v = steerDirection.Normalized() * Speed - velocity;
		return v.Clamp(v, new Vector2(3,3));
	}

	// This is to run when the player is not moving
	private void LoiterBehaviour()
	{

	}

	// When player is moving, it will follow the player
	private void FollowOwnerBehaviour()
	{

	}

	private bool IsWithinDistanceOfPlayer()
	{
		float distanceThreshold = SocialDistancingFromPlayer; // Set your desired distance threshold here
		float distance = GlobalPosition.DistanceTo(birdOwner.GlobalPosition);
		return distance <= distanceThreshold;
	}

#endregion

}
