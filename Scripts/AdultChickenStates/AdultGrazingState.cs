using Godot;

public class AdultGrazingState : AdultChickenBase
{
    private float grazingTime = 0f;
    private float totalGrazeTime = 0f;
    private float GrazingDuration;

    public AdultGrazingState(adult_chick chicken) : base(chicken) { }

    public override void Enter()
    {
        // Set grazing animation or sprite
        GrazingDuration = (float)GD.RandRange(chicken.GrazingDuration.X, chicken.GrazingDuration.Y);
        grazingTime = 0f;
        chicken.ChangeAnimation("Grazing");

    }

    public override void Execute(float delta)
    {
        grazingTime += delta;
        totalGrazeTime += delta;
        if (grazingTime >= 1f)
        {
            grazingTime = 0f;
            chicken.DecreaseHunger(chicken.HungerDecreaseAmount);
        }

        if(totalGrazeTime >= GrazingDuration)
        {
            chicken.IncrementRandomStat();
            chicken.ChangeState(AdultChickenStates.Thinking);
        }
    }

    public override void Exit() { }
}