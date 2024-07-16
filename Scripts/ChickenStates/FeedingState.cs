using Godot;

public class FeedingState : ChickenBase
{
    private float feedTimer = 0f;
    private float FeedingDuration;

    food_item targetFoodItem;

    public FeedingState(ChickBehaviour chick) : base(chick) { }

    public override void Enter()
    {
        // Set sleeping animation or sprite
        feedTimer = 0f;
        chick.ChangeAnimation("Grazing");
    }

    public override void Execute(float delta)
    {
        if (targetFoodItem != null)
        {
            // Vector2 targetPosition = targetFoodItem.GlobalPosition;
            // float distanceToTarget = chick.GlobalPosition.DistanceTo(targetPosition);

            // if (distanceToTarget <= chick.FeedingRange)
            // {
            //     // Within range, start feeding
            //     FeedingDuration = targetFoodItem.FeedingDuration;
            // }
            // else
            // {
            //     // Move towards the target food item
            //     Vector2 direction = (targetPosition - chick.GlobalPosition).Normalized();
            //     Vector2 velocity = direction * chick.MoveSpeed;
            //     chick.MoveAndSlide(velocity);
            // }
        }

        feedTimer += delta;


        if (feedTimer >= FeedingDuration || chick.Fatigue >= chick.FatigueThreshold)
        {
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}