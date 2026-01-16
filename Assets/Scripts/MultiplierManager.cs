using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplierManager : MonoBehaviour
{
    // Every multipliers in the game
    public Upgrades[] multipliers;

    // Bought multipliers by the player
    public Upgrades[] boughtMultipliers;


    void Awake()           
    {
        buildMultipliers();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initMultipliers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void buildMultipliers()
    {
        multipliers = Array.Empty<Upgrades>();
        boughtMultipliers = Array.Empty<Upgrades>();

        initMultipliers();
    }

    void initMultipliers()
    {
        multipliers = multipliers
           .Concat(initClickPowers())
           .Concat(initAutomaticProduction())
           .Concat(initBoosters())
           .Concat(initEndgameBonuses())
           .Concat(initVisualUpgrades())
           .OrderBy(m => m.cost)
           .ToArray();
    }

    Upgrades[] initClickPowers()
    {
        var strongCoffee = new ClickPowers(1,"Strong coffee", "", 15, 2, 0.1);
        strongCoffee.addSprite(Resources.Load<Sprite>("Upgrades/ClickPowers/strongCoffee"));

        var baristaHand = new ClickPowers(2,"Barista's Hand", "", 30, 2, 0.5);
        baristaHand.addSprite(Resources.Load<Sprite>("Upgrades/ClickPowers/baristaHand"));

        return new Upgrades[]
        {
            strongCoffee,
            baristaHand
        };
    }

    Upgrades[] initAutomaticProduction()
    {
        var basicMachine = new Multiplier(3, "Expresso Coffee Machine", "", 150, 1.5, 0.1);
        basicMachine.addSprite(Resources.Load<Sprite>("Upgrades/Multiplier/expressoMachine"));

        var vintagePercolator = new Multiplier(4, "Vintage Percolator", "", 300, 2, 1);
        vintagePercolator.addSprite(Resources.Load<Sprite>("Upgrades/Multiplier/vintagePercolator"));

        var indusTorrefactor = new Multiplier(5, "Industrial Torrefactor", "", 1000, 2, 10);
        // Sprite not added yet

        var nestluckFactory = new Multiplier(6, "Nestluck Factory", "", 2500, 2, 100);
        nestluckFactory.addSprite(Resources.Load<Sprite>("Upgrades/Multiplier/nestluckFactory"));

        var intergalacticCoffee = new Multiplier(7, "Intergalactic Coffee", "", 20000, 2, 1000);
        intergalacticCoffee.addSprite(Resources.Load<Sprite>("Upgrades/Multiplier/intergalacticCoffee"));

        var roboticBarista = new Multiplier(8, "Robotic Barista", "", 2000000, 2, 10000);
        roboticBarista.addSprite(Resources.Load<Sprite>("Upgrades/Multiplier/roboticBarista"));

        return new Upgrades[]
        {
            basicMachine,
            vintagePercolator,
            indusTorrefactor,
            nestluckFactory,
            intergalacticCoffee,
            roboticBarista
        };
    }

    Upgrades[] initBoosters()
    {
        // 1% production per minute (infinite duration)
        var yearlyCustomer = new Bonuses(9, "Customer of the year", "", 100000, 3, 60, 0.01, -1);
        yearlyCustomer.addSprite(Resources.Load<Sprite>("Upgrades/Bonuses/customer-1"));

        // +10% autoproduction
        var kittysBar = new Bonuses(10, "Kitty's Bar", "", 1000000, 2, -1, 0.1, -1);
        kittysBar.addSprite(Resources.Load<Sprite>("Upgrades/Bonuses/kittyBar"));

        // 0.5% automatic production for 15 secondes
        var overcloackedMachine = new Bonuses(11, "Overcloacked Machine", "", 10000, 2, -1, 0.005, 15);
        overcloackedMachine.addSprite(Resources.Load<Sprite>("Upgrades/Bonuses/overcloackedMachine"));

        // *5 autoprod for 10 secondes
        var influenceurCoffee = new Bonuses(12, "Influenceur Coffee", "", 250000, 3, -1, 5, 10);
        // sprite not added yet

        return new Upgrades[]
        {
            yearlyCustomer,
            kittysBar,
            overcloackedMachine,
            influenceurCoffee
        };
    }

    // Special EndGames Bonuses that reset EVERYTHING but give a permanent bonus
    Upgrades[] initEndgameBonuses()
    {
        var blackholeCoffee = new ClickPowers(13, "Black Hole Coffee", "", 10000000, 2, 10000);
        blackholeCoffee.addSprite(Resources.Load<Sprite>("Upgrades/ClickPowers/blackholeCoffee"));

        var coffeeshopCEO = new Multiplier(14, "New Coffee Shop Director", "", 50000000, 2, 1000);
        coffeeshopCEO.addSprite(Resources.Load<Sprite>("Upgrades/Multiplier/coffeeShopCEO"));

        Upgrades[] endGameBonuses = new Upgrades[]
        {
          blackholeCoffee,
          coffeeshopCEO
        };

        for (int i = 0; i < endGameBonuses.Length; i++)
        {
            Bonuses bonus = endGameBonuses[i] as Bonuses;
            if (bonus != null)
            {
                bonus.setEngameBonus();
            }
        }

        return endGameBonuses;
    }

    Upgrades[] initVisualUpgrades()
    {
        return new Upgrades[]
        {
            new Themes(15,"Blue Neons", "", 15000, -1, null),
            new Themes(16,"Rustic Tables", "", 30000, -1, null),
            new Themes(17,"Barista Kitty", "", 50000, -1, null),
        };
    }

    public void purchaseUpgrade(Upgrades upgrade)
    {
        Array.Resize(ref boughtMultipliers, boughtMultipliers.Length + 1);
        boughtMultipliers[boughtMultipliers.Length - 1] = upgrade;
    }

    public void ResetPurchases()
    {
        boughtMultipliers = Array.Empty<Upgrades>();
    }

}
