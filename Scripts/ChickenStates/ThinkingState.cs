using Godot;

public class ThinkingState : ChickenBase
{
    public ThinkingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set thinking animation or sprite
    }

    public override void Execute(float delta)
    {
        // Decide next state based on factors
        if (chick.Fatigue > 80f)
        {
            chick.ChangeState(ChickenStates.Sleeping);
        }
        else if (chick.Hunger > 70f)
        {
            chick.ChangeState(ChickenStates.Grazing);
        }
        else if (chick.Boredom > 60f)
        {
            chick.ChangeState(ChickenStates.Playing);
        }
        else if (GD.Randf() < 0.3f) // 30% chance to relax
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