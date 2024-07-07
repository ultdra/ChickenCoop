public class BabyRelaxingState : BabyChickenBase
{
    private float relaxTime = 0f;
    private const float RelaxDuration = 8f;

    public BabyRelaxingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set relaxing animation or sprite
        relaxTime = 0f;
        chick.ChangeAnimation("Relaxing");
    }

    public override void Execute(float delta)
    {
        relaxTime += delta;
        if (relaxTime >= RelaxDuration)
        {
            chick.DecreaseFatigue(chick.RelaxFatigueDecreaseAmount);
            chick.ChangeState(BabyChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}