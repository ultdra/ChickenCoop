using System.Collections.Generic;
using Godot;

/// <summary>
/// This will be the flesh and bone of the chick's behaviour.
/// 
/// Inspired by the Boid system, we should start seeing chicks run around.
/// 
/// Nearby chicks will be attracted to each other and will start playing.
/// 
/// </summary>
public class PlayingState : ChickenBase
{

    private float playTime = 0f;
    private float PlayDuration;
    private List<baby_chick> playmates;

    // For boids related
    private Vector2 playDirection;

    public PlayingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // if(chick.GetNearestChick() != null)
        // {
        //     playDirection = (chick.GetNearestChick().GlobalPosition - chick.GlobalPosition).Normalized();
        // }
        // else
        // {
            
        // }
        playDirection = (chick.GetRandomPosition() - chick.GlobalPosition).Normalized();
        chick.Velocity = playDirection * chick.PlaySpeed;

        // Set playing animation or sprite
        PlayDuration =  (float)GD.RandRange(chick.PlayDuration.X, chick.PlayDuration.Y);
        playTime = 0f;
        playmates = chick.GetNearbyChicks();

    }

    public override void Execute(float delta)
    {
        chick.Velocity = SeekTarget(baby_chick.MousePosition);

        playTime += delta;

        Vector2 separation = CalculateSeperation();
        Vector2 cohesion = CalculateCohesion();
        Vector2 alignment = CalculateAlignment();


        Vector2 acceleration = separation * chick.AvoidanceFactor  +
                                alignment * chick.AlignmentFactor +
                                cohesion * chick.CohesionFactor;

        chick.Velocity += acceleration * delta;
        // chick.Velocity = AvoidBorders(chick.GlobalPosition, chick.Velocity);


        chick.MoveAndSlide();


        // TryToMotivateOthers();

        if (playTime >= PlayDuration)
        {
            // chick.ChangeState(ChickenStates.Thinking);
            // chick.StartPlayCooldown();
        }

    }

    public override void Exit() { }

    private Vector2 SeekTarget(Vector2 position)
    {
        Vector2 desiredDirection = position - chick.GlobalPosition;
        desiredDirection = desiredDirection.Normalized() * chick.PlaySpeed;

        Vector2 steeringForce = chick.Velocity + desiredDirection;
        steeringForce = steeringForce.Normalized() * chick.PlaySpeed;

        return steeringForce;
    }
    private Vector2 CalculateSeperation()
    {
        Vector2 separate = Vector2.Zero;
        int count = 0;
        var nearbyChicks = chick.GetNearbyChicks();
        foreach (var otherChick in nearbyChicks)
        {
            if(otherChick != chick)
            {
                float distance = chick.GlobalPosition.DistanceTo(otherChick.GlobalPosition);
                if (distance < chick.PlayAttractRange)
                {
                    Vector2 diff = chick.GlobalPosition - otherChick.GlobalPosition;
                    diff = diff.Normalized()/distance;
                    separate += diff;
                    ++count;
                }
            }
        }

        if(count > 0)
        {
            separate = separate.Normalized() * chick.PlaySpeed;
            Vector2 steer = separate - chick.Velocity;
            if(steer.Length() > chick.PlaySpeed)
            {
                steer = steer.Normalized() * chick.PlaySpeed;
            }

            return steer;
        }
        // No adjustment needed
        return Vector2.Zero;
    }

    private Vector2 CalculateAlignment()
    {
        Vector2 sum = Vector2.Zero;
        int count = 0;
        var nearbyChicks = chick.GetNearbyChicks();
        foreach (var otherChick in nearbyChicks)
        {
            float distance = chick.GlobalPosition.DistanceTo(otherChick.GlobalPosition);
            if (distance < chick.PlayAttractRange)
            {
                sum += otherChick.Velocity;
                count++;
            }
        }
        if (count > 0)
        {
            sum /= count; // Calculate the average velocity
            sum = sum.Normalized() * chick.PlaySpeed; // Set the magnitude to max speed
            Vector2 steer = sum - chick.Velocity;
            if(steer.Length() > chick.PlaySpeed)
            {
                steer = steer.Normalized() * chick.PlaySpeed;
            }            return steer;
        }
        return Vector2.Zero; // Return zero vector if no alignment is needed
    }

    private Vector2 CalculateCohesion()
    {
        Vector2 cohesion = Vector2.Zero;
        int count = 0;
        var furthestChicks = chick.GetFurthestChicks();
        foreach (var otherChick in furthestChicks)
        {
            if(otherChick != chick)
            {
                float distance = chick.GlobalPosition.DistanceTo(otherChick.GlobalPosition);
                if (distance > chick.PlayAttractRange)
                {
                    Vector2 diff = otherChick.GlobalPosition - chick.GlobalPosition;
                    diff = diff.Normalized() * distance;
                    cohesion += diff;
                    ++count;
                }
            }
        }

        if(count > 0)
        {
            cohesion = cohesion.Normalized() * chick.PlaySpeed;
            Vector2 steer = cohesion - chick.Velocity;
            if(steer.Length() > chick.PlaySpeed)
            {
                steer = steer.Normalized() * chick.PlaySpeed;
            }
            return steer;
        }
        
        // No adjustment needed
        return Vector2.Zero;
    }

    private Vector2 AvoidBorders(Vector2 position, Vector2 velocity)
    {
        Rect2 bounds = chick.GetTileMapBounds();
        Vector2 desiredDirection = velocity.Normalized();
        float borderDistance = 40.0f; // Distance from border to start avoiding
        float steeringStrength = 0.5f; // Adjust this to change how sharply the chick turns

        // Check left and right borders
        if (position.X - bounds.Position.X < borderDistance)
        {
            desiredDirection += Vector2.Right * steeringStrength;
        }
        else if (bounds.End.X - position.X < borderDistance)
        {
            desiredDirection += Vector2.Left * steeringStrength;
        }

        // Check top and bottom borders
        if (position.Y - bounds.Position.Y < borderDistance)
        {
            desiredDirection += Vector2.Down * steeringStrength;
        }
        else if (bounds.End.Y - position.Y < borderDistance)
        {
            desiredDirection += Vector2.Up * steeringStrength;
        }

        desiredDirection = desiredDirection.Normalized();
        Vector2 steeringForce = (desiredDirection - velocity.Normalized()) * steeringStrength;
        return velocity + steeringForce;
    }

    private void TryToMotivateOthers()
    {
        var nearbyChicks = chick.GetNearbyChicks();
        foreach (var otherChick in nearbyChicks)
        {
            if (otherChick.CanPlay && !(otherChick.CurrentChickState == ChickenStates.Playing))
            {
                otherChick.ChangeState(ChickenStates.Playing);
            }
        }
    }
}