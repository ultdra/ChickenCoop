using Godot;

public class AdultSleepingState : AdultChickenBase
{
    private float sleepTime = 0f;
    private float SleepDuration;

    public AdultSleepingState(adult_chick chicken) : base(chicken) { }

    public override void Enter()
    {
        // Set sleeping animation or sprite
        SleepDuration = (float)GD.RandRange(chicken.SleepDuration.X, chicken.SleepDuration.Y);
        sleepTime = 0f;
        chicken.ChangeAnimation("Sleeping");
    }

    public override void Execute(float delta)
    {
        sleepTime += delta;
        chicken.DecreaseFatigue(delta * chicken.SleepFatigueDecreaseRate);

        if (sleepTime >= SleepDuration || chicken.Fatigue <= chicken.FatigueThreshold)
        {
            chicken.ChangeState(AdultChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}