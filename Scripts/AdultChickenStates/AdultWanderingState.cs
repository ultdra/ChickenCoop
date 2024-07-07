using Godot;

public class AdultWanderingState : AdultChickenBase
{
    private Vector2 targetPosition;
    private float wanderTime = 0f;

    public AdultWanderingState(adult_chick chicken) : base(chicken) { }

    public override void Enter()
    {
        // Set wandering animation or sprite
        targetPosition = chicken.GetRandomPosition();
        wanderTime = 0f;
        chicken.ChangeAnimation("Walking");
    }

    public override void Execute(float delta)
    {
        wanderTime += delta;
        Vector2 direction = (targetPosition - chicken.GlobalPosition).Normalized();
        chicken.GlobalPosition += direction * chicken.WanderSpeed * delta;

        float wanderDuration = (float)GD.RandRange(chicken.WanderDuration.X, chicken.WanderDuration.Y);

        if(direction.X > 0)
        {
            chicken.FlipAnimationDirection(false);
        }
        else if(direction.X < 0)
        {
            chicken.FlipAnimationDirection(true);
        }

        if (chicken.GlobalPosition.DistanceTo(targetPosition) < 5f || wanderTime >= wanderDuration)
        {
            chicken.ChangeState(AdultChickenStates.Thinking);
        }
    }

    public override void Exit() { }

}