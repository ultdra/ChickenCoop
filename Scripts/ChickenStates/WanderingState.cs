using Godot;

public class WanderingState : ChickenBase
{
    private Vector2 targetPosition;
    private float wanderTime = 0f;
    private const float WanderDuration = 7f;

    public WanderingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set wandering animation or sprite
        targetPosition = GetRandomPosition();
        wanderTime = 0f;
    }

    public override void Execute(float delta)
    {
        wanderTime += delta;
        Vector2 direction = (targetPosition - chick.GlobalPosition).Normalized();
        chick.GlobalPosition += direction * 30f * delta;

        if (chick.GlobalPosition.DistanceTo(targetPosition) < 5f || wanderTime >= WanderDuration)
        {
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }

    private Vector2 GetRandomPosition()
    {
        // Implement logic to get a random position within the coop
        return Vector2.Zero; // Placeholder
    }
}