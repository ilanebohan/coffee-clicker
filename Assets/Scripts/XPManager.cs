using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    // GameObject Slider of the XPBar
    public GameObject XPBar;

    public GameObject lvlText;

    // Current XP
    public int currentXP = 0;

    // Multiplier for XP needed for next Lvl
    public float xpMultiplierToNextLvl = 1.5f;

    // Current Lvl
    public int currentLvl = 1;

    // XP Needed for Lvl UP (Default 100 for Lvl 1 -> Lvl 2)
    public int nextlvlXP = 100;


    /*
     *
     * MULTPLIERS 
     * Here u'll find every multipliers : Base XP win for click & AutoClick
     * 
     * And their multipliers : For Click 1 XP, then 1*1.25, etc..
     * For Multipliers 2 XP, then 2*1.25, etc..
     * 
     * Also minimum added Score for multipliers : 50, then 50*2, etc..
     *
     */

    // Minimum Points needed for multiplier to win XP
    public int multiplierMinimumToWinXP = 50;

    // Multiply the min points needed for multipliers each time
    public float multipliersMultiplierForXP = 2f;

    // The min points win for each click
    public int clickMinimumXpWin = 1;

    // The min points win for each multiplier win
    public int multiplierMinimumXpWin = 2;

    // Multiply the xp win for each click or multiplier
    public float multipliersForXP = 1.25f;


    /**
     * Everything about Perk : Also a PerkManager to handle Purchased Perks etc.. 
     * Maybe just a placeholder for the moment
     * */

    // Perks points
    public int perkPoints = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetXpBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentXP >= nextlvlXP)
        {
            this.LevelUp();
        }
    }

    public void resetXpBar()
    {
        //XPBar.GetComponent<Slider>().minValue = currentXP;
        XPBar.GetComponent<Slider>().value = currentXP;
        XPBar.GetComponent<Slider>().maxValue = nextlvlXP;
        lvlText.GetComponent<TextMeshProUGUI>().text = currentLvl.ToString();
    }

    private void AddXP(int xp)
    {
        this.currentXP += xp;
        XPBar.GetComponent<Slider>().value = currentXP;
        //Debug.Log("AddXP" + XPBar.GetComponent<Slider>().value + " - " + currentXP);
    }

    private void LevelUp()
    {
        XPBar.GetComponent<Slider>().minValue = nextlvlXP;
        //Debug.Log("LevelUP 1" + XPBar.GetComponent<Slider>().minValue + " - " + nextlvlXP);
        this.currentLvl += 1;
        this.nextlvlXP = (int)(this.nextlvlXP * this.xpMultiplierToNextLvl);
        XPBar.GetComponent<Slider>().maxValue = nextlvlXP;
        //Debug.Log("LevelUP 2" + XPBar.GetComponent<Slider>().minValue + " - " + nextlvlXP);
        lvlText.GetComponent<TextMeshProUGUI>().text = currentLvl.ToString();
        this.perkPoints++;


        this.clickMinimumXpWin =
            (int)(this.clickMinimumXpWin * this.multipliersForXP);



        this.multiplierMinimumXpWin =
            (int)(this.multiplierMinimumXpWin * this.multipliersForXP);

        //Debug.Log("LevelUP Final" + this.currentLvl + " - " + nextlvlXP);
    }

    public void HandleXPClick()
    {
        this.AddXP(clickMinimumXpWin);
    }

    public void HandleXPMultiplier(double score, double oldScore)
    {
        if ((score + this.multiplierMinimumToWinXP) >= oldScore)
        {
            this.AddXP(this.multiplierMinimumXpWin);
            this.multiplierMinimumToWinXP =
                (int)
                (this.multiplierMinimumToWinXP * this.multipliersMultiplierForXP);
        }
    }
}
