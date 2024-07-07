using Godot;

public class AdultThinkingState : AdultChickenBase
{
    private float thinkingTime = 0f;
    private float thinkingDuration = 0f;
    public AdultThinkingState(adult_chick chicken) : base(chicken) { }

    public override void Enter()
    {
        thinkingTime = 0f;
        thinkingDuration = (float)GD.RandRange(chicken.ThinkingDuration.X, chicken.ThinkingDuration.Y);
        // Set thinking animation or sprite
        chicken.ChangeAnimation("Wandering2");
    }

    public override void Execute(float delta)
    {
        thinkingTime += delta;
        if(thinkingTime < thinkingDuration)
        {
            return;
        }

        if (GD.Randf() < chicken.RelaxChance) // 30% chance to relax
        {
            chicken.ChangeState(AdultChickenStates.Relaxing);
            return;
        }

        // Weights of the actions
        // We will need to only do this when the threshold is above a certain amount
        float totalWeight = 0;
        float hungerWeight;
        float sleepingWeight;
        
        if(chicken.Hunger > chicken.HungerThreshold)
            totalWeight += chicken.Hunger;
            
        if(chicken.Fatigue > chicken.FatigueThreshold)
            totalWeight += chicken.Fatigue;

        if(totalWeight == 0)
        {
            chicken.ChangeState(AdultChickenStates.Wandering);
        }
        else
        {
            hungerWeight = chicken.Hunger / totalWeight;
            sleepingWeight = chicken.Fatigue / totalWeight;

            // Now we need to know whcih state it will enter
            float randomWeight = GD.Randf();

            // Decide next state based on factors
            // -= on the if else to remove the weight from the randomWeight and check on the next state
            if ((randomWeight -= sleepingWeight) <= 0.0f)
            {
                chicken.ChangeState(AdultChickenStates.Sleeping);
            }
            else if ((randomWeight -= hungerWeight) <= 0.0f)
            {
                chicken.ChangeState(AdultChickenStates.Grazing);
            }
        }

    }

    public override void Exit() { }
}