public class AdultRelaxingState : AdultChickenBase
{
    private float relaxTime = 0f;
    private const float RelaxDuration = 8f;

    public AdultRelaxingState(adult_chick chicken) : base(chicken) { }

    public override void Enter()
    {
        // Set relaxing animation or sprite
        relaxTime = 0f;
        chicken.ChangeAnimation("Relaxing");
    }

    public override void Execute(float delta)
    {
        relaxTime += delta;
        if (relaxTime >= RelaxDuration)
        {
            chicken.DecreaseFatigue(chicken.RelaxFatigueDecreaseAmount);
            chicken.ChangeState(AdultChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}