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
    private baby_chick playmate;

    // For boids related
    private Vector2 playDirection;

    public PlayingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        playDirection = (chick.GetRandomPosition() - chick.GlobalPosition).Normalized(); 
        // Set playing animation or sprite
        PlayDuration =  (float)GD.RandRange(playmate.PlayDuration.X, playmate.PlayDuration.Y);
        playTime = 0f;
        playmate = FindNearestChick();

        chick.Velocity = playDirection * chick.PlaySpeed;
    }

    public override void Execute(float delta)
    {
        playTime += delta;

        Vector2 seperation = CalculateSeperation();
        Vector2 alignment = CalculateAlignment();
        Vector2 cohesion = CalculateCohesion();

        Vector2 acceleration = seperation * chick.AvoidanceFactor
                                + alignment * chick.AlignmentFactor
                                + cohesion * chick.CohesionFactor;

        chick.Velocity += acceleration * delta;
        // chick.Velocity = chick.Velocity.Clamp(chick.PlaySpeed);

    }

    public override void Exit() { }

    private baby_chick FindNearestChick()
    {
        // Implement logic to find the nearest chick
        return null; // Placeholder
    }

    private Vector2 CalculateSeperation()
    {
        // Implement logic to calculate separation
        return Vector2.Zero; // Placeholder
    }

    private Vector2 CalculateAlignment()
    {
        // Implement logic to calculate alignment
        return Vector2.Zero; // Placeholder
    }

    private Vector2 CalculateCohesion()
    {
        // Implement logic to calculate cohesion
        return Vector2.Zero; // Placeholder
    }
}