using UnityEngine;

public class Upgrades
{
    public int id;

    public string name;

    public string description;

    // Main sprite of the Upgrade
    public Sprite sprite;

    // Cost of the Upgrade
    public double cost;

    // Upgrade increase rate (how much the cost increases each purchase)
    // if -1 = Buyable 1 time only
    public double costIncreaseMultiplier;

    // If the Upgrade is locked or not
    public bool locked = true;

    // If the Upgrade is an end-game upgrade (/!\ RESET EVERYTHING WHEN BOUGHT /!\)
    public bool isEndGameUpgrade = false;


    public Upgrades(int id, string name, string description, double cost, double costIncreaseMultiplier)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.costIncreaseMultiplier = costIncreaseMultiplier;
    }

    public void unlockUpgrade()
    {
        this.locked = true;
    }

    public void increaseCostMultiplier()
    {
        this.cost *= this.costIncreaseMultiplier;
    }

    public void setEngameBonus()
    {
        this.isEndGameUpgrade = true;
    }

    public void addSprite(Sprite newSprite)
    {
        this.sprite = newSprite;
    }

}
