using Godot;

public class ThinkingState : ChickenBase
{
    private float thinkingTime = 0f;
    private float thinkingDuration = 0f;
    public ThinkingState(baby_chick chick) : base(chick) { }

    public override void Enter()
    {
        thinkingTime = 0f;
        thinkingDuration = (float)GD.RandRange(chick.ThinkingDuration.X, chick.ThinkingDuration.Y);
        // Set thinking animation or sprite
        chick.ChangeAnimation("Wandering2");
    }

    public override void Execute(float delta)
    {
        thinkingTime += delta;
        if(thinkingTime < thinkingDuration)
        {
            return;
        }

        if (GD.Randf() < chick.RelaxChance) // 30% chance to relax
        {
            chick.ChangeState(ChickenStates.Relaxing);
            return;
        }

        // Weights of the actions
        // We will need to only do this when the threshold is above a certain amount
        float totalWeight = 0;
        float hungerWeight;
        float sleepingWeight;
        float boredomWeight;
        
        if(chick.Hunger > chick.HungerThreshold)
            totalWeight += chick.Hunger;
            
        if(chick.Fatigue > chick.FatigueThreshold)
            totalWeight += chick.Fatigue;
            
        if(chick.Boredom > chick.BoredomThreshold)
            totalWeight += chick.Boredom;

        if(totalWeight == 0)
        {
            chick.ChangeState(ChickenStates.Wandering);
        }
        else
        {
            hungerWeight = chick.Hunger / totalWeight;
            sleepingWeight = chick.Fatigue / totalWeight;
            boredomWeight = chick.Boredom / totalWeight;

            // Now we need to know whcih state it will enter
            float randomWeight = GD.Randf();

            // Decide next state based on factors
            // -= on the if else to remove the weight from the randomWeight and check on the next state
            if ((randomWeight -= sleepingWeight) <= 0.0f)
            {
                chick.ChangeState(ChickenStates.Sleeping);
            }
            else if ((randomWeight -= hungerWeight) <= 0.0f)
            {
                chick.ChangeState(ChickenStates.Grazing);
            }
            else if ((randomWeight -= boredomWeight) <= 0.0f)
            {
                chick.ChangeState(ChickenStates.Playing);
            }
        }

    }

    public override void Exit() { }
}