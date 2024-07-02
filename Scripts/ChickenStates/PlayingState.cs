using Godot;

public class PlayingState : ChickenBase
{
    private float playTime = 0f;
    private const float PlayDuration = 10f;
    private baby_chick playmate;

    public PlayingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        // Set playing animation or sprite
        playTime = 0f;
        playmate = FindNearestChick();
    }

    public override void Execute(float delta)
    {
        playTime += delta;
        if (playmate != null)
        {
            // Implement chasing logic here
            Vector2 direction = (playmate.GlobalPosition - chick.GlobalPosition).Normalized();
            chick.GlobalPosition += direction * 50f * delta;
        }

        if (playTime >= PlayDuration)
        {
            chick.DecreaseBoredom(chick.BoredomPlayThreshold);
            chick.ChangeState(ChickenStates.Thinking);
        }
    }

    public override void Exit() { }

    private baby_chick FindNearestChick()
    {
        // Implement logic to find the nearest chick
        return null; // Placeholder
    }
}