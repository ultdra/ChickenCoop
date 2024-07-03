using Godot;
using System;
using System.Collections.Generic;

public partial class baby_chick : CharacterBody2D
{
    [ExportGroup("Decay Rates")]
    [Export] 
    public float HungerDecayRate { get; set; } = 0.1f;
    [Export] 
    public float BoredomDecayRate { get; set; } = 0.15f;
    [Export] 
    public float FatigueDecayRate { get; set; } = 0.05f;

    [ExportGroup("Thresholds")]
    [Export] 
    public float FatigueThreshold { get; set; } = 80f;
    [Export] 
    public float HungerThreshold { get; set; } = 70f;
    [Export] 
    public float BoredomThreshold { get; set; } = 60f;
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

    [ExportGroup("Play State")]
    [Export] 
    public Vector2 PlayDuration { get; set; } = new Vector2(15f, 30f);
    [Export] 
    public float BoredomDecreaseAmount { get; set; } = 30f;
    [Export] 
    public float PlaySpeed { get; set; } = 50f;
    [Export]
    public float PlayCooldown { get; set; } = 300f;
    [Export]
    public float PlayAttractRange { get; set; } = 100f;

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

    [ExportGroup("Boid System")]
    [Export] 
    public float AvoidanceFactor { get; set; } = 80f;
    [Export] 
    public float AlignmentFactor { get; set; } = 70f;
    [Export] 
    public float CohesionFactor { get; set; } = 60f;


    // Behaviour related
    public float Hunger { get; private set; } = 0f;
    public float Boredom { get; private set; } = 0f;
    public float Fatigue { get; private set; } = 0f;

    // State Machine
    private Dictionary<ChickenStates, ChickenBase> states;
    private ChickenStates currentChickenState = ChickenStates.Thinking;

    private ChickenStats stats = new ChickenStats();

    public bool CanPlay { get; private set; } = true;
    private float playCooldownTimer = 0f;

    // For animation
    private AnimatedSprite2D animationController;

    // State timers
    private float stateTimer = 0;
    public ChickenStates CurrentChickState => currentChickenState;

    // Debug labels
    private Label nameLabel;
    private Label stateLabel;
    private Label hungerLabel;
    private Label boredomLabel;
    private Label fatigueLabel;


    public override void _Ready()
    {
        nameLabel = GetNode<Label>("NameLabel");
        stateLabel = GetNode<Label>("StateLabel");
        hungerLabel = GetNode<Label>("HungerLabel");
        boredomLabel = GetNode<Label>("BoredomLabel");
        fatigueLabel = GetNode<Label>("FatigueLabel");

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
        if(!CanPlay)
        {
            playCooldownTimer += (float)delta;
            if(playCooldownTimer >= PlayCooldown)
            {
                CanPlay = true;
                playCooldownTimer = 0f;
            }
        }

        UpdateFactors((float)delta);
        states[currentChickenState].Execute((float)delta);
    }

    private void UpdateFactors(float delta)
    {
        Hunger = (float)Math.Min(Hunger + HungerDecayRate * delta, 100f);
        Boredom = (float)Math.Min(Boredom + BoredomDecayRate * delta, 100f);
        Fatigue = (float)Math.Min(Fatigue + FatigueDecayRate * delta, 100f);
        hungerLabel.Text = $"Hunger: {Hunger.ToString("F0")}";
        boredomLabel.Text = $"Boredom: {Boredom.ToString("F0")}";
        fatigueLabel.Text = $"Fatigue: {Fatigue.ToString("F0")}";
    }

    public void ChangeState(ChickenStates newState)
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

    public Rect2 GetTileMapBounds()
    {
        TileMap tileMap = GetNode<TileMap>("../Level");

        if (tileMap == null)
        {
            GD.PrintErr("TileMap not set for Chick");
            return new Rect2(0, 0, 1, 1);
        }

        Rect2 usedRect = tileMap.GetUsedRect();
        Vector2 tileSize = tileMap.TileSet.TileSize;
        Vector2 tileMapScale = tileMap.Scale;

        float minX = usedRect.Position.X * tileSize.X * tileMapScale.X;
        float minY = usedRect.Position.Y * tileSize.Y * tileMapScale.Y;
        float maxX = (usedRect.Position.X + usedRect.Size.X) * tileSize.X * tileMapScale.X;
        float maxY = (usedRect.Position.Y + usedRect.Size.Y) * tileSize.Y * tileMapScale.Y;

        return new Rect2(minX, minY, maxX - minX, maxY - minY);
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

    public List<baby_chick> GetNearbyChicks()
    {
        List<baby_chick> nearbyChicks = new List<baby_chick>();
        foreach (Node2D chick in GetTree().GetNodesInGroup("Chicks"))
        {
            if (chick != this)
            {
                float distance = GlobalPosition.DistanceTo(chick.GlobalPosition);
                if (distance <= PlayAttractRange)
                {
                    nearbyChicks.Add(chick as baby_chick);
                }
            }
        }
        return nearbyChicks;
    }

    public void StartPlayCooldown()
    {
        CanPlay = false;
        playCooldownTimer = 0f;
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
