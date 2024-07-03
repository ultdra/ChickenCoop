using Godot;

public class GrazingState : ChickenBase
{
    private float grazingTime = 0f;
    private float totalGrazeTime = 0f;
    private float GrazingDuration;

    public GrazingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set grazing animation or sprite
        GrazingDuration = (float)GD.RandRange(chick.GrazingDuration.X, chick.GrazingDuration.Y);
        grazingTime = 0f;
        chick.ChangeAnimation("Grazing");

    }

    public override void Execute(float delta)
    {
        grazingTime += delta;
        totalGrazeTime += delta;
        if (grazingTime >= 1f)
        {
            grazingTime = 0f;
            chick.DecreaseHunger(chick.HungerDecreaseAmount);
        }

        if(totalGrazeTime >= GrazingDuration)
        {
            chick.IncrementRandomStat();
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}