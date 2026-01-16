using UnityEngine;

public class Multiplier : Upgrades
{
    // How much it adds to autoclicks
    public double autoclicksMultiplierValue;

    public Multiplier(int id, string name, string description, double cost, double costIncreaseMultiplier,
        double autoclicksMultiplierValue) 
        : base(id, name, description, cost, costIncreaseMultiplier)
    {
        this.autoclicksMultiplierValue = autoclicksMultiplierValue;
    }
}
