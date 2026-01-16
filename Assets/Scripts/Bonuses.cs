using UnityEngine;

public class Bonuses : Upgrades
{

    // Loop time for boosters in seconds (for boosters)
    public double boosterLoopTime;

    // Multiplier value for boosters (How much it multiplies production for a duration)
    public double boosterMultiplierValue;

    // Duration of the multiplier effect in seconds (for boosters)
    public double duration;

    public Bonuses(int id, string name, string description, double cost, double costIncreaseMultiplier,
        double boosterLoopTime, double boosterMultiplierValue, double duration) 
        : base(id, name, description, cost, costIncreaseMultiplier)
    {
        this.boosterMultiplierValue = boosterMultiplierValue;
        this.boosterLoopTime = boosterLoopTime;
        this.duration = duration;
    }

    public double GetFactorFromBonus()
    {
        // Si >= 1 → on considère que c'est déjà un facteur (ex: 5 = *5)
        // Si < 1 → on considère que c'est un pourcentage (0.1 = +10% → facteur 1.1)
        return this.boosterMultiplierValue >= 1.0
            ? this.boosterMultiplierValue
            : 1.0 + this.boosterMultiplierValue;
    }
}
