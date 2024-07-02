using Godot;
using System;

public partial class egg : AnimatedSprite2D
{
    private double hatchTime = 180; // 3 minutes
    private double timer = 0.0f;

    public override void _Process(double delta)
    {
        timer += delta;
        if (timer >= hatchTime)
        {
            Hatch();
        }
    }

    private void Hatch()
    {
        // Instantiate a Chick at this position
        // Remove this Egg
    }
}
