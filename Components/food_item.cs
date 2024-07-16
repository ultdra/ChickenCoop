using Godot;
using System;

public partial class food_item : Node2D
{
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




}
