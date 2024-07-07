using Godot;

public class BabySleepingState : BabyChickenBase
{
    private float sleepTime = 0f;
    private float SleepDuration;

    public BabySleepingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set sleeping animation or sprite
        SleepDuration = (float)GD.RandRange(chick.SleepDuration.X, chick.SleepDuration.Y);
        sleepTime = 0f;
        chick.ChangeAnimation("Sleeping");
    }

    public override void Execute(float delta)
    {
        sleepTime += delta;
        chick.DecreaseFatigue(delta * chick.SleepFatigueDecreaseRate);

        if (sleepTime >= SleepDuration || chick.Fatigue <= chick.FatigueThreshold)
        {
            chick.ChangeState(BabyChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}