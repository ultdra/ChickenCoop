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
        playDirection = (chick.GetRandomPosition() - chick.GlobalPosition).Normalized(); 
        // Set playing animation or sprite
        PlayDuration =  (float)GD.RandRange(chick.PlayDuration.X, chick.PlayDuration.Y);
        playTime = 0f;
        playmates = chick.GetNearbyChicks();

        chick.Velocity = playDirection * chick.PlaySpeed;
    }

    public override void Execute(float delta)
    {
            playTime += delta;

            Vector2 separation = CalculateSeperation();
            Vector2 alignment = CalculateAlignment();
            Vector2 cohesion = CalculateCohesion();

            Vector2 acceleration = separation * chick.AvoidanceFactor +
                                alignment * chick.AlignmentFactor +
                                cohesion * chick.CohesionFactor;

            chick.Velocity += acceleration * delta;
            chick.Velocity = AvoidBorders(chick.GlobalPosition, chick.Velocity);
            
            Vector2 newPosition = chick.GlobalPosition + chick.Velocity * delta;
            chick.GlobalPosition = newPosition;

            TryToMotivateOthers();

            if (playTime >= PlayDuration)
            {
                chick.ChangeState(ChickenStates.Thinking);
                chick.StartPlayCooldown();
            }

    }

    public override void Exit() { }

    private Vector2 CalculateSeperation()
    {
        Vector2 separate = Vector2.Zero;
        var nearbyChicks = chick.GetNearbyChicks();
        foreach (var otherChick in nearbyChicks)
        {
            Vector2 diff = chick.GlobalPosition - otherChick.GlobalPosition;
            separate += diff.Normalized() / diff.Length();
        }
        return separate;
    }

    private Vector2 CalculateAlignment()
    {
        Vector2 align = Vector2.Zero;
        var nearbyChicks = chick.GetNearbyChicks();
        if (nearbyChicks.Count > 0)
        {
            foreach (var otherChick in nearbyChicks)
            {
                align += otherChick.Velocity;
            }
            align /= nearbyChicks.Count;
        }
        return align;
    }

    private Vector2 CalculateCohesion()
    {
        Vector2 cohesion = Vector2.Zero;
        var nearbyChicks = chick.GetNearbyChicks();
        if (nearbyChicks.Count > 0)
        {
            foreach (var otherChick in nearbyChicks)
            {
                cohesion += otherChick.GlobalPosition;
            }
            cohesion /= nearbyChicks.Count;
            cohesion = (cohesion - chick.GlobalPosition).Normalized();
        }
        return cohesion;
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