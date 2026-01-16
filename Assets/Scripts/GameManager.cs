using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    // Actual score
    public double score = 0;
    // Points per seconds : Only for Multiplier and Bonuses
    public double autoClickValue = 0;
    // If the click speed is faster (?)
    public double clickSpeed;

    [Header("Managers")]
    // Every multipliers in the game
    public MultiplierManager multipliersManager;
    public Click clickManager;
    public UpgradeMenu upgradeMenu;
    public XPManager xpManager;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;

    // Events
    public event Action<int> OnScoreChanged;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("GameManager init");
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        AddScorePerSeconds();
    }

    public void AddScore()
    {
        score += clickManager.clickValue;
        UpdateScoreText();
        OnScoreChanged?.Invoke((int)score);
        xpManager.HandleXPClick();
    }

    public void AddScorePerSeconds()
    {
        if (autoClickValue > 0)
        {
            double oldScore = score;
            score += autoClickValue * Time.deltaTime;
            UpdateScoreText();
            OnScoreChanged?.Invoke((int)score);

            xpManager.HandleXPMultiplier(score, oldScore);
        }
    }


    public void purchaseUpgradeNoCost(Upgrades up)
    {
        //score -= up.cost;
        //UpdateScoreText();
        //OnScoreChanged?.Invoke((int)score);
        if (up is ClickPowers)
        {
            clickManager.clickValue += ((ClickPowers)up).clicksMultiplierValue;
        }
        else if (up is Multiplier)
        {
            autoClickValue += ((Multiplier)up).autoclicksMultiplierValue;
        }
        else if (up is Bonuses)
        {

            // Cas 1 : durée infinie
            if (((Bonuses)up).duration < 0)
            {
                if (((Bonuses)up).boosterLoopTime < 0)
                {
                    // Bonus permanent, appliqué une seule fois
                    ApplyMultiplier(((Bonuses)up));
                }
                else
                {
                    // Bonus infini qui s'applique en boucle (ex: +1% / minute)
                    StartCoroutine(ApplyInfiniteLoopBonus(((Bonuses)up)));
                }
            }
            // Cas 2 : durée limitée
            else
            {
                // Bonus temporaire appliqué une fois puis retiré
                StartCoroutine(ApplyTimedBonus(((Bonuses)up)));
            }

        }
        // Increase the cost
        up.cost *= up.costIncreaseMultiplier;

        multipliersManager.purchaseUpgrade(up);
    }

    public void purchaseUpgrade(Upgrades up)
    {
            score -= up.cost;
            UpdateScoreText();
            OnScoreChanged?.Invoke((int)score);
        if (up is ClickPowers)
            {
                clickManager.clickValue += ((ClickPowers)up).clicksMultiplierValue;
            }
            else if (up is Multiplier)
            {
                autoClickValue += ((Multiplier)up).autoclicksMultiplierValue;
            }
            else if (up is Bonuses)
            {

                // Cas 1 : durée infinie
                if (((Bonuses)up).duration < 0)
                {
                    if (((Bonuses)up).boosterLoopTime < 0)
                    {
                        // Bonus permanent, appliqué une seule fois
                        ApplyMultiplier(((Bonuses)up));
                    }
                    else
                    {
                        // Bonus infini qui s'applique en boucle (ex: +1% / minute)
                        StartCoroutine(ApplyInfiniteLoopBonus(((Bonuses)up)));
                    }
                }
                // Cas 2 : durée limitée
                else
                {
                    // Bonus temporaire appliqué une fois puis retiré
                    StartCoroutine(ApplyTimedBonus(((Bonuses)up)));
                }

        }
        // Increase the cost
        up.cost *= up.costIncreaseMultiplier;

        multipliersManager.purchaseUpgrade(up);
    }

    public void UpdateScoreText()
    {
        string displayScore = score % 1 == 0
            ? ((int)score).ToString()
            : score.ToString("F1");

        scoreText.text = displayScore + " coffee(s)";
    }

    private void ApplyMultiplier(Bonuses bonus)
    {
        double factor = bonus.GetFactorFromBonus();
        autoClickValue *= factor;
    }

    private IEnumerator ApplyTimedBonus(Bonuses bonus)
    {
        double factor = bonus.GetFactorFromBonus();

        // On applique le bonus
        autoClickValue *= factor;

        // On attend la durée
        yield return new WaitForSeconds((float)bonus.duration);

        // On enlève le bonus (on divise par le même facteur)
        autoClickValue /= factor;
    }

    private IEnumerator ApplyInfiniteLoopBonus(Bonuses bonus)
    {
        double factor = bonus.GetFactorFromBonus();

        while (true)
        {
            autoClickValue *= factor;
            yield return new WaitForSeconds((float)bonus.boosterLoopTime);
        }
    }



}
