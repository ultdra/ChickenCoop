using Godot;
using System;

public partial class egg : AnimatedSprite2D
{
    [Export]
    private double hatchTime = 180; // 3 minutes
    private double timer = 0.0f;

    private bool hatched = false;

    PackedScene chickScene;
    public override void _Ready()
    {
        chickScene = ResourceLoader.Load<PackedScene>("res://Components/UpgradeCoop/baby_chick.tscn");
        Connect("animation_finished", new Callable(this, nameof(OnHatchFinish)), 0);
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

    private void OnHatchFinish()
    {
        // Spawn another gameobject
        var chickInstance = chickScene.Instantiate() as Node2D;
        GetParent().AddChild(chickInstance);
        chickInstance.Position = Position; 

        // Destroy this gameobject
        QueueFree();

    }
}
