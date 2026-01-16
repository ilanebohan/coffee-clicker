using UnityEngine;

public class ClickPowers : Upgrades
{
    // Multiplier value (How much it adds to clicks)
    public double clicksMultiplierValue;

    public ClickPowers(int id,string name, string description, double cost, double costIncreaseMultiplier,
        double clicksMultiplierValue) : base(id, name, description, cost, costIncreaseMultiplier)
    {
        this.clicksMultiplierValue = clicksMultiplierValue;
    }

}
