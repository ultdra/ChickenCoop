using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class bird : CharacterBody2D
{
	private float maxSpeed = 150.0f;
    private float maxForce = 8.0f;
    private float perceptionRadius = 100.0f;
    private float separationRadius = 50.0f;
    private float comfortZone = 125.0f; // Distance to maintain from leader
    private float rotationSpeed = 20.0f;
    private float surroundRadius = 60.0f; // Radius to surround the leader
    private float flowFieldStrength = 0.5f; // Strength of the flow field effect

	private BirdState birdState = BirdState.None;

    private CharacterBody2D leader;
    private List<bird> flock;
    private AnimatedSprite2D animationController;

    public override void _Ready()
    {
        leader = GetNode<CharacterBody2D>("/root/Main/Player");
        flock = GetTree().GetNodesInGroup("Bird").Cast<bird>().ToList();
		animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Ensure playerNode is not null
		if (leader != null)
		{
			// Notify that player has a new unit tagging along
			leader.Connect(nameof(player.PlayerIdle), new Callable(this, "OnPlayerIdle"), 0);
			leader.Connect(nameof(player.PlayerRunning), new Callable(this, "OnPlayerRunning"), 0);
			leader.Connect(nameof(player.PlayerDead), new Callable(this, "OnPlayerDead"), 0);
		}

    }

    public override void _PhysicsProcess(double delta)
    {

        if (!IsWithinDistanceOfPlayer())
		{
			animationController.Animation = "Run";
			animationController.Play();
			birdState = BirdState.FollowOwner;
		}


		switch(birdState)
		{
			case BirdState.Idle:
				break;

			case BirdState.ApproachOwner :
				break;
			
			case BirdState.FollowOwner:
                Vector2 acceleration = Vector2.Zero;

                Vector2 alignment = Align();
                Vector2 cohesion = Cohere();
                Vector2 separation = Separate();
                Vector2 follow = FollowLeader();
                Vector2 flowField = GetFlowField();

                acceleration += alignment * 1.0f;
                acceleration += cohesion * 1.0f;
                acceleration += separation * 3.0f;
                acceleration += follow * 1.5f;
                acceleration += flowField * flowFieldStrength;

                acceleration = acceleration.LimitLength(maxForce);
                Velocity += acceleration;
                Velocity = Velocity.LimitLength(maxSpeed);

                MoveAndSlide();
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

#region boird
    private Vector2 Align()
    {
        Vector2 steering = Vector2.Zero;
        int total = 0;
        foreach (var boid in flock)
        {
            if (boid != this && GlobalPosition.DistanceTo(boid.GlobalPosition) < perceptionRadius)
            {
                steering += boid.Velocity;
                total++;
            }
        }
        if (total > 0)
        {
            steering /= total;
            steering = steering.Normalized() * maxSpeed;
            steering -= Velocity;
        }
        return steering;
    }

    private Vector2 Cohere()
    {
        Vector2 steering = Vector2.Zero;
        int total = 0;
        foreach (var boid in flock)
        {
            if (boid != this && GlobalPosition.DistanceTo(boid.GlobalPosition) < perceptionRadius)
            {
                steering += boid.GlobalPosition;
                total++;
            }
        }
        if (total > 0)
        {
            steering /= total;
            steering = (steering - GlobalPosition).Normalized() * maxSpeed;
            steering -= Velocity;
        }
        return steering;
    }

 private Vector2 Separate()
    {
        Vector2 steering = Vector2.Zero;
        int total = 0;
        foreach (var boid in flock)
        {
            float d = GlobalPosition.DistanceTo(boid.GlobalPosition);
            if (boid != this && d < separationRadius)
            {
                Vector2 diff = GlobalPosition - boid.GlobalPosition;
                diff /= d * d;
                if (d < separationRadius / 2)
                {
                    diff *= 2;
                }
                steering += diff;
                total++;
            }
        }
        if (total > 0)
        {
            steering /= total;
            steering = steering.Normalized() * maxSpeed;
            steering -= Velocity;
        }
        return steering;
    }

    private Vector2 FollowLeader()
    {
        Vector2 toLeader = leader.GlobalPosition - GlobalPosition;
        float distanceToLeader = toLeader.Length();

        // Calculate a position around the leader
        float angle = GlobalPosition.AngleToPoint(leader.GlobalPosition);
        angle += Mathf.Pi / 2; // Offset by 90 degrees to orbit around the leader
        Vector2 targetPosition = leader.GlobalPosition + Vector2.Right.Rotated(angle) * surroundRadius;

        Vector2 desired = targetPosition - GlobalPosition;
        float distance = desired.Length();

        if (distance > comfortZone)
        {
            desired = desired.Normalized() * maxSpeed;
            Vector2 steering = desired - Velocity;
            return steering;
        }
        else
        {
            // If within comfort zone, slow down
            return -Velocity * (1 - distance / comfortZone);
        }
    }

    private Vector2 GetFlowField()
    {
        // Create a flow field that encourages sliding past other boids
        Vector2 flow = Vector2.Zero;
        foreach (var boid in flock)
        {
            if (boid != this)
            {
                Vector2 offset = boid.GlobalPosition - GlobalPosition;
                if (offset.Length() < perceptionRadius)
                {
                    flow += offset.Rotated(Mathf.Pi / 2).Normalized(); // Rotate 90 degrees to create a perpendicular flow
                }
            }
        }
        return flow.Normalized();
    }
#endregion

#region Signals
	private void OnPlayerIdle()
	{
		animationController.Animation = "Idle";
		animationController.Play();
		birdState = BirdState.Idle;
	}
	
	private void OnPlayerRunning()
	{
		animationController.Animation = "Run";
		animationController.Play();
		birdState = BirdState.FollowOwner;
	}

	private void OnPlayerDead()
	{

	}
#endregion


#region private manipulators
	private bool IsWithinDistanceOfPlayer()
	{
		Vector2 desired = leader.GlobalPosition - GlobalPosition;
        float distance = desired.Length();

        if (distance > comfortZone)
        {
            desired = desired.Normalized() * maxSpeed;
            Vector2 steering = desired - Velocity;
            return false;
        }

		return true;
	}
#endregion
}