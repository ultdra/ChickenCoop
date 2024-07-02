using Godot;
using System;

public partial class BirdManager : Node2D
{
    private PackedScene followerScene;
    private int numFollowers = 10;

    public override void _Ready()
    {
        // For transparent background
        // GetTree().Root.TransparentBg = true;
        // DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Transparent, true, 0);

        followerScene = GD.Load<PackedScene>("res://Components/bird.tscn");

        GetNode<Node2D>("/root/Main/Player").AddToGroup("leader");
        for (int i = 0; i < numFollowers; i++)
        {
            bird follower = followerScene.Instantiate<bird>();
            AddChild(follower);
            follower.Scale = new Vector2(2,2);

            follower.AddToGroup("Bird");

            follower.Position = new Vector2((float)GD.RandRange(0, (GetViewportRect().Size/2).X), (float)GD.RandRange(0, (GetViewportRect().Size/2).Y));

            // Add debug log
        }
    }
}