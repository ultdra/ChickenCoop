using Godot;
using System;

public partial class GlobalInventoryManager : Node
{
    #region Custom Signals

    public delegate void InventoryUpdateDelegate();

    public event InventoryUpdateDelegate OnInventoryUpdate;

    #endregion

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


    public void AddItem()
    {
        EmitSignal("OnInventoryUpdate");
    }

    public void RemoveItem()
    {
        EmitSignal("OnInventoryUpdate");
    }

}



