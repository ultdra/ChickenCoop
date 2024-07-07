using Godot;
using System;
using System.Collections.Generic;

public partial class adult_chick : CharacterBody2D
{
    [ExportGroup("Decay Rates")]
    [Export] 
    public float HungerDecayRate { get; set; } = 0.1f;
    [Export] 
    public float FatigueDecayRate { get; set; } = 0.05f;

    [ExportGroup("Thresholds")]
    [Export] 
    public float FatigueThreshold { get; set; } = 80f;
    [Export] 
    public float HungerThreshold { get; set; } = 70f;
    [Export] 
    public float RelaxChance { get; set; } = 0.3f;

    [ExportGroup("Thinking State")]
    [Export]
    public Vector2 ThinkingDuration { get; set; } = new Vector2(3f, 7f);

    [ExportGroup("Grazing State")]
    [Export] 
    public Vector2 GrazingDuration { get; set; } = new Vector2(15f, 30f);
    [Export] 
    public float HungerDecreaseAmount { get; set; } = 20f;
    [Export] 
    public int GrazeStatIncreaseAmount { get; set; } = 1;

    [ExportGroup("Fatigue State")]
    [Export] 
    public Vector2 RelaxDuration { get; set; } = new Vector2(10f, 20f);
    [Export] 
    public float RelaxFatigueDecreaseAmount { get; set; } = 10f;
    [Export] 
    public Vector2 SleepDuration { get; set; } = new Vector2(15f, 30f);
    [Export] 
    public float SleepFatigueDecreaseRate { get; set; } = 5f;

    [ExportGroup("Wandering State")]
    [Export] 
    public Vector2 WanderDuration { get; set; } = new Vector2(5f, 10f);
    [Export] 
    public float WanderSpeed { get; set; } = 30f;

    // Behaviour related
    public float Hunger { get; private set; } = 0f;
    public float Fatigue { get; private set; } = 0f;

    // State Machine
    private Dictionary<AdultChickenStates, AdultChickenBase> states;
    private AdultChickenStates currentChickenState = AdultChickenStates.Thinking;

    private ChickenStats stats = new ChickenStats();

    // For animation
    private AnimatedSprite2D animationController;

    // State timers
    private float stateTimer = 0;
    public AdultChickenStates CurrentChickState => currentChickenState;

    // Debug labels
    private Label nameLabel;
    private Label stateLabel;
    private Label hungerLabel;
    private Label boredomLabel;
    private Label fatigueLabel;

    public override void _Ready()
    {
        animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        states = new Dictionary<AdultChickenStates, AdultChickenBase>
        {
            { AdultChickenStates.Thinking, new AdultThinkingState(this) },
            { AdultChickenStates.Wandering, new AdultWanderingState(this) },
            { AdultChickenStates.Grazing, new AdultGrazingState(this) },
            { AdultChickenStates.Relaxing, new AdultRelaxingState(this) },
            { AdultChickenStates.Sleeping, new AdultSleepingState(this) }
        };

        ChangeState(AdultChickenStates.Thinking);
    }

    public override void _Process(double delta)
    {
        UpdateFactors((float)delta);
        states[currentChickenState].Execute((float)delta);
    }

    private void UpdateFactors(float delta)
    {
        if(currentChickenState != AdultChickenStates.Grazing)
            Hunger = (float)Math.Min(Hunger + HungerDecayRate * delta, 100f);

        if(currentChickenState != AdultChickenStates.Sleeping)
            Fatigue = (float)Math.Min(Fatigue + FatigueDecayRate * delta, 100f);

        hungerLabel.Text = $"Hunger: {Hunger.ToString("F0")}";
        fatigueLabel.Text = $"Fatigue: {Fatigue.ToString("F0")}";
    }

    public void ChangeState(AdultChickenStates newState)
    {        
        // GD.Print("Exiting state: " + currentChickenState.ToString());
        // GD.Print("Entering state: " + newState.ToString());
        states[currentChickenState].Exit();
        currentChickenState = newState;
        states[currentChickenState].Enter();
        stateLabel.Text = newState.ToString();
    }

    public void DecreaseHunger(float amount)
    {
        Hunger = Math.Max(Hunger - amount, 0f);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lookLeft"> False to look Right, True to look Left </param>
    public void FlipAnimationDirection(bool LookLeft)
    {
        animationController.FlipH = LookLeft;
    }

    public void InheritBabyChickStats(ChickenStats stats)
    {
        this.stats = stats;
    }

    public void IncrementRandomStat()
    {
        string[] statTypes = { "strength", "agility", "vitality" };
        string randomStat = statTypes[GD.Randi() % 3];
        stats.IncreaseStat(randomStat, 1);
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
    
        float randomX = (float)GD.RandRange(minX, maxX);
        float randomY = (float)GD.RandRange(minY, maxY);
        
        Vector2 randomPosition = new Vector2(randomX, randomY);
        return randomPosition;
    }
}
