using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalInventoryManager : Node
{
    #region Custom Signals

    public delegate void InventoryUpdateDelegate();

    public event InventoryUpdateDelegate OnInventoryUpdate;

    #endregion

    private List<food_item> FoodInventory = new List<food_item>();

    private string foodItemPath = "res://Components/food_item.tscn";
    private PackedScene foodItemPackedScene;

    #region Accessors

    public PackedScene FoodItemPackedScene => foodItemPackedScene;

    #endregion

    public override void _Ready()
    {
        base._Ready();
        foodItemPackedScene = ResourceLoader.Load<PackedScene>(foodItemPath);
    }


    public void AddItem(food_item foodItem, int amount)
    {
        for(int i = 0; i < FoodInventory.Count; ++i)
        {
            if(FoodInventory[i] != null && FoodInventory[i].ItemName == foodItem.ItemName)
            {
                FoodInventory[i].IncreaseFoodCount(amount);
                EmitSignal("OnInventoryUpdate");
                break;
            }
            else if(FoodInventory[i] == null)
            {
                FoodInventory[i] = foodItem;
                EmitSignal("OnInventoryUpdate");
                break;
            }
        }
    }

    public void RemoveItem()
    {
        EmitSignal("OnInventoryUpdate");
    }

}



