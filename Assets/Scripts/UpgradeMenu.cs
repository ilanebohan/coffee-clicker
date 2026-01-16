using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public GameObject upgradeItemPrefab; // ton prefab "UpgradeItem"
    public Transform contentParent;       // le Content du Scroll View
    public MultiplierManager multiplierManager; // référence au MultiplierManager
    public GameManager gameManager;

    private readonly Dictionary<int, UpgradeItemView> _viewsById = new();

    void OnEnable()
    {
        BuildOnceIfNeeded();
        // écoute les changements de score (crée cet event si besoin)
        gameManager.OnScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
       gameManager.OnScoreChanged -= OnScoreChanged;
    }

    private void BuildOnceIfNeeded()
    {
        if (_viewsById.Count > 0) return;

        foreach (var up in multiplierManager.multipliers)
        {
            var go = Instantiate(upgradeItemPrefab, contentParent, false);
            var view = go.GetComponent<UpgradeItemView>();
            view.SetData(up);
            view.OnPurchasedUpgrade += OnPurchase;
            _viewsById[up.id] = view;
        }
        RefreshLayout();
    }

    public void OnScoreChanged(int newScore)
    {
        Debug.Log($"Score changé : {newScore}");

        bool changed = false;

        foreach (var up in multiplierManager.multipliers)
        {
            if (newScore >= up.cost)
            {
                up.locked = false;
                if (_viewsById.TryGetValue(up.id, out var view))
                {
                    view.SetUnlocked(up);
                    // option : view.transform.SetSiblingIndex(0);
                    changed = true;
                }
            }
            else if (newScore < up.cost)
            {
                up.locked = true;
                if (_viewsById.TryGetValue(up.id, out var view))
                {
                    view.SetUnlocked(up);
                    changed = true;
                }
            }
        }

        if (changed) RefreshLayout();
    }

    private void OnPurchase(Upgrades upgrade)
    {
        //Debug.Log($"On achète l'upgrade : {upgrade.name}");
        if (!upgrade.locked && gameManager.score >= upgrade.cost)
        { 
            if (_viewsById.TryGetValue(upgrade.id, out var view))
            {
                gameManager.purchaseUpgrade(upgrade);
                view.SetData(upgrade);
                view.IncreaseOwned();
                RefreshLayout();
            }
        }
    }

    public void LoadingPurchase(Upgrades upgrade)
    {
        if (_viewsById.TryGetValue(upgrade.id, out var view))
        {
            view.SetData(upgrade);
            view.IncreaseOwned();
            RefreshLayout();
        }
    }

    // Forcer un recalcul de layout fiable
    private void RefreshLayout()
    {
        Canvas.ForceUpdateCanvases();
        var rt = (RectTransform)contentParent;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        Canvas.ForceUpdateCanvases();
    }

    // Si tu veux garder ta méthode existante (respawn), garde-la mais évite de l'appeler à chaque tick.
    public void ReloadLayoutHard()
    {
        foreach (Transform child in contentParent) Destroy(child.gameObject);

        foreach (var up in multiplierManager.multipliers)
        {
            var go = Instantiate(upgradeItemPrefab, contentParent, false);
            var view = go.GetComponent<UpgradeItemView>();
            view.SetData(up);
            _viewsById[up.id] = view;
        }
        RefreshLayout();
    }
}
