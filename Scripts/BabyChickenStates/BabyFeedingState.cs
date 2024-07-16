using Godot;

public class BabyFeedingState : BabyChickenBase
{
    private float feedTimer = 0f;
    private float FeedingDuration;
    
    food_item targetFoodItem;

    public BabyFeedingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set sleeping animation or sprite
        feedTimer = 0f;
        chick.ChangeAnimation("Grazing");
    }

    public override void Execute(float delta)
    {
        feedTimer += delta;


        if (feedTimer >= FeedingDuration || chick.Fatigue >= chick.FatigueThreshold)
        {
            chick.ChangeState(BabyChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}