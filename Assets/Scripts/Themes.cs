using UnityEngine;

public class Themes : Upgrades
{
    public GameObject themePrefab;
    public Themes(int id, string name, string description, double cost, double costIncreaseMultiplier, 
        GameObject themePrefab) : base(id, name, description, cost, costIncreaseMultiplier)
    {
        this.themePrefab = themePrefab;
    }

}
