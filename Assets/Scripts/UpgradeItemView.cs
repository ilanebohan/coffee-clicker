using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemView : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public GameObject lockOverlay; // ton Image semi-transparente + icône cadenas
    public Image lockIcon; // le petit cadenas dans l’icône
    public Button buyButton;


    private Upgrades upgradeData;

    // Events
    public event Action<Upgrades> OnPurchasedUpgrade;


    public void SetData(Upgrades upgrade)
    {
        upgradeData = upgrade;
        SetTextData(upgrade);
        SetUnlocked(upgrade);
    }

    public void SetTextData(Upgrades upgrade)
    {
        nameText.text = upgrade.name;
        costText.text = upgrade.cost.ToString("F0");
    }

    public void SetUnlocked(Upgrades upgrade)
    {
        Image img = buyButton.GetComponent<Image>();
        // Choisir quelle image afficher
        if (!upgrade.locked)
        {
            iconImage.sprite = upgrade.sprite;
            iconImage.enabled = true;
            lockOverlay.SetActive(false);
            lockIcon.enabled = false;
            buyButton.enabled = true;
            SetImageAlpha(img, 150);
            SetImageAlpha(iconImage, 150);
            //Debug.Log("On unlock l'upgrade: " + upgrade.name);
        }
        else
        {
            // cadenas = sprite fixe importé ou icône UI
            buyButton.enabled = false;
            SetImageAlpha(img, 0);
            iconImage.enabled = false;
            lockOverlay.SetActive(true);
            lockIcon.enabled = true;
            //Debug.Log("On lock l'upgrade: " + upgrade.name);
        }
    }

    private void Start()
    {
        buyButton.onClick.AddListener(() => OnPurchasedUpgrade?.Invoke(upgradeData));
    }

    public void SetImageAlpha(Image img, int alpha)
    {
        if (img != null)
        {
            Color color = img.color;
            color.a = alpha / 255f;
            img.color = color;
        }
    }
}
