using Godot;
using System;

public partial class baby_chick : CharacterBody2D
{

    [Export]
    private double GrazeDuration = 120;

    [Export]
    private double SleepDuration = 120;

    [Export]
    private double RelaxDuration = 120;

    [Export]
    private double HungerDecayRate = 120;

    [Export]
    private double BoredomDecayRate = 120;

    [Export]
    private double FatigueDecayRate = 120;

    private ChickenStates currentChickenState = ChickenStates.None;
    private ChickenStates previousChickenState = ChickenStates.None;

    private ChickenStats stats = new ChickenStats();

    // State timers
    private double stateTimer = 0;

    public override void _Ready()
    {
        ChangeState(ChickenStates.Wandering);
    }

    public override void _Process(double delta)
    {

        
    }

    private void ChangeState(ChickenStates state)
    {
        // GD.Print(state.ToString());
        currentChickenState = state;
    }

    private void IncrementRandomStat()
    {
        string[] statTypes = { "strength", "agility", "vitality" };
        string randomStat = statTypes[GD.Randi() % 3];
        stats.IncreaseStat(randomStat, 1);

        if (stats.TotalStats >= ChickenStats.MaxStats)
        {
            GrowToAdult();
        }
    }

    private void GrowToAdult()
    {
        
        // Implement visual changes and behavior switches
    }

    private void Thinking()
    {

    }

    private void Wandering()
    {

    }

    private void Grazing(double delta)
    {
        stateTimer += delta;
        if (stateTimer >= GrazeDuration) // 2 minutes
        {
            IncrementRandomStat();
            stateTimer = 0f;
        }
    }

    private void Relaxing(double delta)
    {
        stateTimer += delta;
        if (stateTimer >= GrazeDuration) // 2 minutes
        {
            IncrementRandomStat();
            stateTimer = 0f;
        }
    }

    private void Sleeping(double delta)
    {
        stateTimer += delta;
        if (stateTimer >= GrazeDuration) // 2 minutes
        {
            IncrementRandomStat();
            stateTimer = 0f;
        }
    }


}
