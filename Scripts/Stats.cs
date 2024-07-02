using System;

public class ChickenStats
{
    public int Strength { get; private set; }
    public int Agility { get; private set; }
    public int Vitality { get; private set; }

    public int TotalStats => Strength + Agility + Vitality;

    public const int MaxStats = 500;

    public void IncreaseStat(string stat, int amount)
    {
        switch (stat.ToLower())
        {
            case "strength":
                Strength = Math.Min(Strength + amount, MaxStats - Agility - Vitality);
                break;
            case "agility":
                Agility = Math.Min(Agility + amount, MaxStats - Strength - Vitality);
                break;
            case "vitality":
                Vitality = Math.Min(Vitality + amount, MaxStats - Strength - Agility);
                break;
        }
    }

    public void IncreaseStrength(int amount)
    {
        Strength = Math.Min(Strength + amount, MaxStats - Agility - Vitality);
    }

    public void IncreaseAgility(int amount)
    {
        Agility = Math.Min(Agility + amount, MaxStats - Strength - Vitality);
    }

    public void IncreaseVitality(int amount)
    {
        Vitality = Math.Min(Vitality + amount, MaxStats - Strength - Agility);
    }
}