using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// This will act as an inventory item and also as an ingame item
/// </summary>
public partial class food_item : Node2D
{
    [Export]
    private Area2D FoodAttractCollider;

    private ItemType itemType = ItemType.None;

    private string itemName = "";

    private Texture itemTexture;

    private int itemStackCount = 0;

    // For feeding the chickens
    private int totalFoodAmount;

    // Special effects maybe here. but future problem

    #region Accessors
    public string ItemName => itemName;

    #endregion
    

    public override void _Ready()
    {
        base._Ready();

        // Connect signals to Area2D onbodyentered
        FoodAttractCollider.Connect("body_entered", new Callable(this, nameof(NotifyFoodPlace)), 0);

    }

    public void IncreaseFoodCount(int amount)
    {
        itemStackCount += amount;
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
        GD.Print("Entered notified food");
        // // Get the list of nearest objects in the "Chicks" group
        // var chicks = GetTree().GetNodesInGroup("Chicks");

        // // Get the list of nearest objects in the "Chickens" group
        // var chickens = GetTree().GetNodesInGroup("Chickens");

        // // Combine the lists of chicks and chickens
        // var nearestObjects = new List<Node>(chicks);
        // nearestObjects.AddRange(chickens);

        // // Do something with the nearest objects
        // foreach (Node2D obj in nearestObjects)
        // {
        //     // Check if the object is within the FoodAttractDistance
        //     if (GlobalPosition.DistanceTo(obj.GlobalPosition) <= FoodAttractDistance)
        //     {
        //         // Your logic here
        //         GD.Print(obj.Name);
        //     }
        // }
    }




}
