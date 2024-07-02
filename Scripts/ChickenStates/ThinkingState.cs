using Godot;

public class ThinkingState : ChickenBase
{
    public ThinkingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set thinking animation or sprite
        chick.ChangeAnimation("Wandering2");
    }

    public override void Execute(float delta)
    {
        // Decide next state based on factors
        if (chick.Fatigue > chick.FatigueSleepThreshold)
        {
            chick.ChangeState(ChickenStates.Sleeping);
        }
        else if (chick.Hunger > chick.HungerGrazeThreshold)
        {
            chick.ChangeState(ChickenStates.Grazing);
        }
        else if (chick.Boredom > chick.BoredomPlayThreshold)
        {
            chick.ChangeState(ChickenStates.Playing);
        }
        else if (GD.Randf() < chick.RelaxChance) // 30% chance to relax
        {
            chick.ChangeState(ChickenStates.Relaxing);
        }
        else
        {
            chick.ChangeState(ChickenStates.Wandering);
        }
    }

    public override void Exit() { }
}