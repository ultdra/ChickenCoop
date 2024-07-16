using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class food_item : Node2D
{
    [Export]
    private float FoodAttractDistance = 100f;

    private ItemType itemType = ItemType.None;

    private string itemName = "";

    private Texture itemTexture;

    private int totalFoodAmount;

    // Special effects maybe here. but future problem

    

    public override void _Ready()
    {
        base._Ready();




    }


    /// <summary>
    /// This should also return a "Effect" pack for the chick to consume and take effect
    /// </summary>
    /// <param name="amount"> The amount of food the chick has eaten</param>
    public void ConsumeFood(int amount)
    {
        totalFoodAmount -= amount;
    }

    /// <summary>
    /// We will emit a signal to let the chicks know that there is food.
    /// </summary>
    private void NotifyFoodPlace()
    {
        // Get the list of nearest objects in the "Chicks" group
        var chicks = GetTree().GetNodesInGroup("Chicks");

        // Get the list of nearest objects in the "Chickens" group
        var chickens = GetTree().GetNodesInGroup("Chickens");

        // Combine the lists of chicks and chickens
        var nearestObjects = new List<Node>(chicks);
        nearestObjects.AddRange(chickens);

        // Do something with the nearest objects
        foreach (Node2D obj in nearestObjects)
        {
            // Check if the object is within the FoodAttractDistance
            if (GlobalPosition.DistanceTo(obj.GlobalPosition) <= FoodAttractDistance)
            {
                // Your logic here
                GD.Print(obj.Name);
            }
        }
    }




}
