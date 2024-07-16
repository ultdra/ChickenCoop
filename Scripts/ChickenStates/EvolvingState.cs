using Godot;

public class EvolvingState : ChickenBase
{

    public EvolvingState(ChickBehaviour chick) : base(chick) { }

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