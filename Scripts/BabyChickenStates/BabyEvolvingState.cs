using Godot;

public class BabyEvolvingState : BabyChickenBase
{

    public BabyEvolvingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set wandering animation or sprite
        chick.ChangeAnimation("Evolve");
    }

    public override void Execute(float delta)
    {
    }

    public override void Exit() { }

}