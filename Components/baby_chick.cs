using Godot;
using System;
using System.Collections.Generic;

public partial class baby_chick : CharacterBody2D
{

    [Export] 
    public float HungerDecayRate { get; set; } = 0.1f;
    [Export] 
    public float BoredomDecayRate { get; set; } = 0.15f;
    [Export] 
    public float FatigueDecayRate { get; set; } = 0.05f;

    [Export] 
    public float GrazingDuration { get; set; } = 5f;
    [Export] 
    public float HungerDecreaseAmount { get; set; } = 20f;
    [Export] 
    public int GrazeStatIncreaseAmount { get; set; } = 1;

    [Export] 
    public float PlayDuration { get; set; } = 10f;
    [Export] 
    public float BoredomDecreaseAmount { get; set; } = 30f;
    [Export] 
    public float PlaySpeed { get; set; } = 50f;

    [Export] 
    public float RelaxDuration { get; set; } = 8f;
    [Export] 
    public float RelaxFatigueDecreaseAmount { get; set; } = 10f;

    [Export] 
    public float SleepDuration { get; set; } = 15f;
    [Export] 
    public float SleepFatigueDecreaseRate { get; set; } = 5f;

    [Export] 
    public float WanderDuration { get; set; } = 7f;
    [Export] 
    public float WanderSpeed { get; set; } = 30f;

    [Export] 
    public float FatigueThreshold { get; set; } = 80f;
    [Export] 
    public float HungerThreshold { get; set; } = 70f;
    [Export] 
    public float BoredomThreshold { get; set; } = 60f;
    [Export] 
    public float RelaxChance { get; set; } = 0.3f;

    // Behaviour related
    public float Hunger { get; private set; } = 0f;
    public float Boredom { get; private set; } = 0f;
    public float Fatigue { get; private set; } = 0f;

    // State Machine
    private Dictionary<ChickenStates, ChickenBase> states;
    private ChickenStates currentChickenState = ChickenStates.Thinking;

    private ChickenStats stats = new ChickenStats();

    // For animation
    private AnimatedSprite2D animationController;

    // State timers
    private float stateTimer = 0;

    public override void _Ready()
    {
        animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

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
        Console.WriteLine("Hunger: " + Hunger);
        Console.WriteLine("Boredom: " + Boredom);
        Console.WriteLine("Fatigue: " + Fatigue);
    }

    public void ChangeState(ChickenStates newState)
    {
        GD.Print("Exiting state: " + currentChickenState.ToString());
        GD.Print("Entering state: " + newState.ToString());
        states[currentChickenState].Exit();
        currentChickenState = newState;
        states[currentChickenState].Enter();
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

    public void ChangeAnimation(string animationName)
    {
        animationController.Animation = animationName;
        animationController.Play();
    }

    public Vector2 GetRandomPosition()
    {
        TileMap tileMap = GetNode<TileMap>("../Level");
        Rect2 borders = tileMap.GetUsedRect();
        Vector2 tileSize = tileMap.TileSet.TileSize;
    
        float minX = borders.Position.X * tileSize.X;
        float maxX = (borders.Position.X + borders.Size.X) * tileSize.X * tileMap.Scale.X;

        float minY = borders.Position.Y * tileSize.Y;
        float maxY = (borders.Position.Y + borders.Size.Y) * tileSize.Y * tileMap.Scale.Y;

        // GD.Print("minX: " + minX.ToString());
        // GD.Print("maxX: " + maxX.ToString());
        // GD.Print("minY: " + minY.ToString());
        // GD.Print("maxY: " + maxY.ToString());
    
        float randomX = (float)GD.RandRange(minX, maxX);
        float randomY = (float)GD.RandRange(minY, maxY);
        
        Vector2 randomPosition = new Vector2(randomX, randomY);
        GD.Print(Name + " is going to " + randomPosition.ToString());
        return randomPosition;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lookLeft"> False to look Right, True to look Left </param>
    public void FlipAnimationDirection(bool LookLeft)
    {
        animationController.FlipH = LookLeft;
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
