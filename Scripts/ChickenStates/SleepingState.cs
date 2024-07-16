using Godot;

public class SleepingState : ChickenBase
{
    private float sleepTime = 0f;
    private float SleepDuration;

    public SleepingState(ChickBehaviour chick) : base(chick) { }

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
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}