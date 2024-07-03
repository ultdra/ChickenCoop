using Godot;
using System;

public partial class egg : AnimatedSprite2D
{
    [Export]
    private Vector2 hatchTimeRange = new Vector2(10f,30f); // 3 minutes

    private double timer = 0.0f;

    private bool hatched = false;
    private float hatchTime = 0.0f;

    // UI Components
    private Label nameLabel;
    private Label debugHatchTimeLabel;

    PackedScene chickScene;
    public override void _Ready()
    {
        nameLabel = GetNode<Label>("NameLabel");
        debugHatchTimeLabel = GetNode<Label>("HatchTimeLabel");
        hatchTime = (float)GD.RandRange(hatchTimeRange.X, hatchTimeRange.Y);
        chickScene = ResourceLoader.Load<PackedScene>("res://Components/baby_chick.tscn");
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
        Node2D chickInstance = chickScene.Instantiate() as Node2D;
        chickInstance.AddToGroup("Chicks");
        GetParent().AddChild(chickInstance);
        chickInstance.Position = Position; 
        chickInstance.Scale = new Vector2(3,3);
        // Destroy this gameobject
        QueueFree();

    }
}
