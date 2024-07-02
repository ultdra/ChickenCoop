using Godot;
using System;

public partial class egg : AnimatedSprite2D
{
    [Export]
    private double hatchTime = 180; // 3 minutes
    private double timer = 0.0f;

    private bool hatched = false;

    public override void _Ready()
    {
        Animation = "Incubating";
		Play();
    }

    public override void _Process(double delta)
    {
        if(hatched) return;

        timer += delta;
        if (timer >= hatchTime)
        {
            Hatch();
        }
    }

    private void Hatch()
    {
        hatched = true;
        // Instantiate a Chick at this position
        // Remove this Egg
        Animation = "Hatching";
		Play();
    }
}
