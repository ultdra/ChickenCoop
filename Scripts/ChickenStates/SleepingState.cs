public class SleepingState : ChickenBase
{
    private float sleepTime = 0f;
    private const float SleepDuration = 15f;

    public SleepingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set sleeping animation or sprite
        sleepTime = 0f;
        chick.ChangeAnimation("Sleeping");
    }

    public override void Execute(float delta)
    {
        sleepTime += delta;
        chick.DecreaseFatigue(delta * chick.SleepFatigueDecreaseRate);

        if (sleepTime >= SleepDuration || chick.Fatigue <= chick.FatigueSleepThreshold)
        {
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}