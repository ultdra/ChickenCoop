using Godot;
using System;
using System.Collections.Generic;

public partial class baby_chick : CharacterBody2D
{

    [Export]
    private float GrazeDuration = 120;

    [Export]
    private float SleepDuration = 120;

    [Export]
    private float RelaxDuration = 120;

    [Export]
    private float HungerDecayRate = 120;

    [Export]
    private float BoredomDecayRate = 120;

    [Export]
    private float FatigueDecayRate = 120;

    // Behaviour related
    public float Hunger { get; private set; } = 0f;
    public float Boredom { get; private set; } = 0f;
    public float Fatigue { get; private set; } = 0f;

    // State Machine
    private Dictionary<ChickenStates, ChickenBase> states;
    private ChickenStates currentChickenState = ChickenStates.Thinking;

    private ChickenStats stats = new ChickenStats();

    // State timers
    private float stateTimer = 0;

    public override void _Ready()
    {
        states = new Dictionary<ChickenStates, ChickenBase>
        {
            { ChickenStates.Thinking, new ThinkingState(this) },
            { ChickenStates.Wandering, new WanderingState(this) },
            { ChickenStates.Grazing, new GrazingState(this) },
            { ChickenStates.Playing, new PlayingState(this) },
            { ChickenStates.Relaxing, new RelaxingState(this) },
            { ChickenStates.Sleeping, new SleepingState(this) }
        };

        ChangeState(ChickenStates.Thinking);
    }

    public override void _Process(double delta)
    {
        UpdateFactors((float)delta);
        states[currentChickenState].Execute((float)delta);
    }

    private void UpdateFactors(float delta)
    {
        Hunger = (float)Math.Min(Hunger + HungerDecayRate * delta, 100f);
        Boredom = (float)Math.Min(Boredom + BoredomDecayRate * delta, 100f);
        Fatigue = (float)Math.Min(Fatigue + FatigueDecayRate * delta, 100f);
    }

    public void ChangeState(ChickenStates newState)
    {
        states[currentChickenState].Exit();
        currentChickenState = newState;
        states[currentChickenState].Enter();
        GD.Print("Exiting state: " + currentChickenState.ToString());
        GD.Print("Entering state: " + newState.ToString());
    }

    public void DecreaseHunger(float amount)
    {
        Hunger = Math.Max(Hunger - amount, 0f);
    }

    public void DecreaseBoredom(float amount)
    {
        Boredom = Math.Max(Boredom - amount, 0f);
    }

    public void DecreaseFatigue(float amount)
    {
        Fatigue = Math.Max(Fatigue - amount, 0f);
    }

    public void IncrementRandomStat()
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



}
