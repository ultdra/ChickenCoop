public class SleepingState : ChickenBase
{
    private float sleepTime = 0f;
    private const float SleepDuration = 15f;

    public SleepingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set sleeping animation or sprite
        sleepTime = 0f;
    }

    public override void Execute(float delta)
    {
        sleepTime += delta;
        chick.DecreaseFatigue(delta * 5f);

        if (sleepTime >= SleepDuration || chick.Fatigue <= 0f)
        {
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}