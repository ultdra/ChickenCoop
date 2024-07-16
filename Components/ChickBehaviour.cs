using Godot;
using System;
using System.Collections.Generic;

public partial class ChickBehaviour : CharacterBody2D
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
    [Export]
    public int TotalStatsBeforeEvolving {get; private set; } = 500;

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

    [ExportGroup("Feeding State")]
    [Export]
    public Vector2 FeedingDuration { get; set; } = new Vector2(15f, 30f);
    [Export]
    public int FeedEatenAmount { get; set; } = 5;


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
    [Export]
    public float PlayRunToRange {get; set;} = 500f;

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
    [Export]
    public float SeparationDistance { get; set; } = 50f;
    [Export]
    public float SteeringFactor { get; set; } = 0.5f;


    // Behaviour related
    public float Hunger { get; private set; } = 0f;
    public float Boredom { get; private set; } = 0f;
    public float Fatigue { get; private set; } = 0f;
    public int TotalStats => stats.TotalStats;

    // State Machine
    private Dictionary<ChickenStates, ChickenBase> states;
    private ChickenStates currentChickenState = ChickenStates.Thinking;

    private ChickenStats stats = new ChickenStats();
    private ChickenStage currentGrowthStage = ChickenStage.Egg;

    public bool CanPlay { get; private set; } = true;
    private float playCooldownTimer = 0f;
    public bool MotivatedPlay {get; private set;} = false;

    // For animation
    private AnimatedSprite2D animationController;

    // State timers
    private float stateTimer = 0;

    // Acessors
    public ChickenStates CurrentChickState => currentChickenState;
    public ChickenStage CurrentGrowthState => currentGrowthStage;

    // For instantiating the adult chicken
    PackedScene adultChickenScene;

    // Debug labels
    private Label nameLabel;
    private Label stateLabel;
    private Label hungerLabel;
    private Label boredomLabel;
    private Label fatigueLabel;


    public override void _Ready()
    {

        adultChickenScene = ResourceLoader.Load<PackedScene>("res://Components/adult_chick.tscn");

        nameLabel = GetNode<Label>("NameLabel");
        stateLabel = GetNode<Label>("StateLabel");
        hungerLabel = GetNode<Label>("HungerLabel");
        boredomLabel = GetNode<Label>("BoredomLabel");
        fatigueLabel = GetNode<Label>("FatigueLabel");

        animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animationController.Connect("animation_finished", new Callable(this, nameof(GrowToAdult)), 0);

        states = new Dictionary<ChickenStates, ChickenBase>
        {
            { ChickenStates.Thinking, new ThinkingState(this) },
            { ChickenStates.Wandering, new WanderingState(this) },
            { ChickenStates.Grazing, new GrazingState(this) },
            { ChickenStates.Playing, new PlayingState(this) },
            { ChickenStates.Relaxing, new RelaxingState(this) },
            { ChickenStates.Sleeping, new SleepingState(this) },
            { ChickenStates.Feeding, new SleepingState(this) },
            { ChickenStates.Evolving, new EvolvingState(this) },
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
        if(currentChickenState != ChickenStates.Grazing)
            Hunger = (float)Math.Min(Hunger + HungerDecayRate * delta, 100f);

        if(currentChickenState != ChickenStates.Playing)
            Boredom = (float)Math.Min(Boredom + BoredomDecayRate * delta, 100f);

        if(currentChickenState != ChickenStates.Sleeping)
            Fatigue = (float)Math.Min(Fatigue + FatigueDecayRate * delta, 100f);

        hungerLabel.Text = $"Hunger: {Hunger.ToString("F0")}";
        boredomLabel.Text = $"Boredom: {Boredom.ToString("F0")}";
        fatigueLabel.Text = $"Fatigue: {Fatigue.ToString("F0")}";
    }

#region Public state and stats manipulators
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

    public void MotivatedToPlay()
    {
        MotivatedPlay = true;
        ChangeState(ChickenStates.Playing);
    }

    public void StartPlayCooldown()
    {
        MotivatedPlay = false;
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
    }

#endregion

    private void GrowToAdult()
    {
        // Implement visual changes and behavior switches
        // Spawn another gameobject
        Node2D chickInstance = adultChickenScene.Instantiate() as Node2D;
        chickInstance.AddToGroup("Chickens");
        GetParent().AddChild(chickInstance);
        chickInstance.Position = Position; 
        chickInstance.Scale = new Vector2(3,3);
        
        // Do something to the adult chicken

        // Destroy this gameobject
        QueueFree();
    }


#region Public Getters for positions
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

    public food_item GetNearestFood()
    {
        food_item nearestFood = null;
        float nearestDistance = float.MaxValue;
        foreach (food_item food in GetTree().GetNodesInGroup("Food"))
        {
            float distance = GlobalPosition.DistanceTo(food.GlobalPosition);
            if (distance <= nearestDistance) 
            {
                nearestFood = food;
            }
        }
        return nearestFood;
    }

    public List<ChickBehaviour> GetNearbyChicks()
    {
        List<ChickBehaviour> nearbyChicks = new List<ChickBehaviour>();
        foreach (Node2D chick in GetTree().GetNodesInGroup("Chicks"))
        {
            if (chick != this)
            {
                float distance = GlobalPosition.DistanceTo(chick.GlobalPosition);
                if (distance <= PlayAttractRange)
                {
                    nearbyChicks.Add(chick as ChickBehaviour);
                }
            }
        }
        return nearbyChicks;
    }

    public List<ChickBehaviour> GetFurthestChicks()
    {
        List<ChickBehaviour> furthestChicks = new List<ChickBehaviour>();

        foreach (Node2D chick in GetTree().GetNodesInGroup("Chicks"))
        {
            if (chick != this)
            {
                float distance = GlobalPosition.DistanceTo(chick.GlobalPosition);
                if (distance >= PlayAttractRange)
                {
                    furthestChicks.Add(chick as ChickBehaviour);
                }
            }
        }

        return furthestChicks;
    }

    public ChickBehaviour GetNearestChick()
    {
        ChickBehaviour nearestChick = null;
        float nearestDistance = float.MaxValue;

        foreach (Node2D chick in GetTree().GetNodesInGroup("Chicks"))
        {
            if (chick != this)
            {
                float distance = GlobalPosition.DistanceTo(chick.GlobalPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestChick = chick as ChickBehaviour;
                }
            }
        }

        return nearestChick;
    }

        public List<ChickBehaviour> GetMostCrowdedQuadrant()
    {
        Rect2 tileMapBounds = GetTileMapBounds();

        int numQuadrantsX = 2; // Only 2 quadrants in X direction
        int numQuadrantsY = 2; // Only 2 quadrants in Y direction

        int[,] quadrantCounts = new int[numQuadrantsX, numQuadrantsY];
 
        foreach (ChickBehaviour chick in GetTree().GetNodesInGroup("Chicks"))
        {
            if (chick != this && chick.CanPlay && !chick.MotivatedPlay) // Only count baby_chicks with MotivatedPlay = false
            {
                Vector2 chickPosition = chick.GlobalPosition - tileMapBounds.Position;
                int quadrantX = (int)(chickPosition.X / (tileMapBounds.Size.X / numQuadrantsX));
                int quadrantY = (int)(chickPosition.Y / (tileMapBounds.Size.Y / numQuadrantsY));

                if (quadrantX >= 0 && quadrantX < numQuadrantsX && quadrantY >= 0 && quadrantY < numQuadrantsY)
                {
                    quadrantCounts[quadrantX, quadrantY]++;
                }
            }
        }

        int maxCount = 0;
        Vector2 mostCrowdedQuadrant = Vector2.Zero;

        for (int X = 0; X < numQuadrantsX; X++)
        {
            for (int Y = 0; Y < numQuadrantsY; Y++)
            {
                if (quadrantCounts[X, Y] > maxCount)
                {
                    maxCount = quadrantCounts[X, Y];
                    mostCrowdedQuadrant = new Vector2(X, Y);
                }
            }
        }

        List<ChickBehaviour> chicksInQuadrant = new List<ChickBehaviour>();

        foreach (ChickBehaviour chick in GetTree().GetNodesInGroup("Chicks"))
        {
            Vector2 chickPosition = chick.GlobalPosition - tileMapBounds.Position;
            int quadrantX = (int)(chickPosition.X / (tileMapBounds.Size.X / numQuadrantsX));
            int quadrantY = (int)(chickPosition.Y / (tileMapBounds.Size.Y / numQuadrantsY));

            if (quadrantX == mostCrowdedQuadrant.X && quadrantY == mostCrowdedQuadrant.Y)
            {
                chicksInQuadrant.Add(chick);
            }
        }

        return chicksInQuadrant;
    }
#endregion

}
