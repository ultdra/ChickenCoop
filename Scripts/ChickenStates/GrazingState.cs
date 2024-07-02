using Godot;

public class GrazingState : ChickenBase
{
    private float grazingTime = 0f;
    private const float GrazingDuration = 5f;

    public GrazingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set grazing animation or sprite
        grazingTime = 0f;
    }

    public override void Execute(float delta)
    {
        grazingTime += delta;
        if (grazingTime >= GrazingDuration)
        {
            chick.DecreaseHunger(20f);
            chick.IncrementRandomStat();
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}