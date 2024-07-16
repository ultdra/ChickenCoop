using Godot;

public class WanderingState : ChickenBase
{
    private Vector2 targetPosition;
    private float wanderTime = 0f;

    public WanderingState(ChickBehaviour chick) : base(chick) { }

    public override void Enter()
    {
        // Set wandering animation or sprite
        targetPosition = chick.GetRandomPosition();
        wanderTime = 0f;
        chick.ChangeAnimation("Walking");
    }

    public override void Execute(float delta)
    {
        wanderTime += delta;
        Vector2 direction = (targetPosition - chick.GlobalPosition).Normalized();
        chick.GlobalPosition += direction * chick.WanderSpeed * delta;

        float wanderDuration = (float)GD.RandRange(chick.WanderDuration.X, chick.WanderDuration.Y);

        if(direction.X > 0)
        {
            chick.FlipAnimationDirection(false);

        }
        else if(direction.X < 0)
        {
            chick.FlipAnimationDirection(true);
        }

        if (chick.GlobalPosition.DistanceTo(targetPosition) < 5f || wanderTime >= wanderDuration)
        {
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }

}