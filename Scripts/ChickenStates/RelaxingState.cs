public class RelaxingState : ChickenBase
{
    private float relaxTime = 0f;
    private const float RelaxDuration = 8f;

    public RelaxingState(baby_chick chick) : base(chick) { }

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
            chick.DecreaseFatigue(chick.RelaxFatigueDecrease);
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}