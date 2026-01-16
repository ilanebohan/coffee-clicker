using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager I;
    [SerializeField] private float autosaveIntervalSeconds = 120f;

    private float _t;
    private bool _dirty;

    public GameManager gameManager;
    public GameTimer gameTimer;
    public XPManager xpManager;
    public MultiplierManager multiplierManager;
    public UpgradeMenu upgradeMenu;


    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
    void OnApplicationQuit() => Save("autosave_force_closing");


    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    private void Update()
    {
        _t += Time.unscaledDeltaTime; // important si menu pause met timeScale=0
        if (_t >= autosaveIntervalSeconds)
        {
            _t = 0f;
            AutoSaveTick();
        }
    }

    public void MarkDirty() => _dirty = true;

    private void AutoSaveTick()
    {
        if (!_dirty) return;
        Save("autosave");
    }

    public void SaveButtonPressed() => Save("manual_button");

    public void QuitButtonPressed()
    {
        Save("quit");
        Application.Quit();
    }

    public void Save(string reason)
    {
        try
        {
            var data = BuildSaveData();
            var json = JsonUtility.ToJson(data, prettyPrint: false);

            var dir = Path.GetDirectoryName(SavePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var tmpPath = SavePath + ".tmp";
            File.WriteAllText(tmpPath, json);

            // Replace atomique (Windows) + fallback
            if (File.Exists(SavePath))
                File.Replace(tmpPath, SavePath, null);
            else
                File.Move(tmpPath, SavePath);

            _dirty = false;
            // Debug.Log($"Saved ({reason}) -> {SavePath}");
        }
        catch (Exception e)
        {
            Debug.LogError("Save failed: " + e);
        }
    }

    public void Load()
    {
        try
        {
            if (!File.Exists(SavePath)) return;
            var json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<GameSaveData>(json);
            ApplySaveData(data);
            _dirty = false;
        }
        catch (Exception e)
        {
            Debug.LogError("Load failed: " + e);
        }
    }

    private GameSaveData BuildSaveData()
    {
        Dictionary<string, int> upgradeCounts = new Dictionary<string, int>();

        foreach (Upgrades boughtMultiplier in multiplierManager.boughtMultipliers)
        {

            if (boughtMultiplier == null) continue;

            string id = boughtMultiplier.id.ToString();

            if (string.IsNullOrEmpty(id))
            {
                continue;
            }

            if (!upgradeCounts.ContainsKey(id))
                upgradeCounts[id] = 0;

            upgradeCounts[id]++;

        }

        UpgradeSave[] upgrades = upgradeCounts
            .Select(kv => new UpgradeSave
            {
                id = kv.Key,
                numbers = kv.Value
            })
            .ToArray();

        // TODO: récupère tes variables (argent, upgrades, etc.)
        return new GameSaveData
        {
            saveId = System.Guid.NewGuid().ToString("N"),
            version = 1,
            savedAtUtc = DateTime.UtcNow.ToString("o"),
            build = Application.version,

            coins = gameManager.score,
            currentTime = gameTimer.currentTime,
            currentLvl = xpManager.currentLvl,
            currentXP = xpManager.currentXP,

            boughtUpgrades = upgrades

        };
    }

    private void ApplySaveData(GameSaveData data)
    {
        if (data == null) return;

        // 1) Etat simple
        gameManager.score = data.coins;
        gameTimer.currentTime = data.currentTime;
        xpManager.currentLvl = data.currentLvl;
        xpManager.currentXP = data.currentXP;

        gameTimer.UpdateUI();
        xpManager.resetXpBar();
        upgradeMenu.OnScoreChanged((int)gameManager.score);

        // 3) Rejouer les upgrades
        if (data.boughtUpgrades != null)
        {
            foreach (var u in data.boughtUpgrades)
            {
                if (string.IsNullOrEmpty(u.id) || u.numbers <= 0) continue;

                for (int i = 0; i < u.numbers; i++)
                {
                    gameManager.purchaseUpgradeNoCost(multiplierManager.multipliers.Where(i => i.id.ToString() == u.id).FirstOrDefault());
                    upgradeMenu.LoadingPurchase(multiplierManager.multipliers.Where(i => i.id.ToString() == u.id).FirstOrDefault());
                }
            }
        }

    }
}

[Serializable]
public class GameSaveData
{
    public string saveId;
    public int version;
    public string savedAtUtc;
    public string build;
    public string checksum;

    public double coins;
    public float currentTime; // Timer of the save
    public int currentLvl;
    public int currentXP;

    public UpgradeSave[] boughtUpgrades; // multipliers : id & occurrences
}

[Serializable]
public struct UpgradeSave
{
    public string id;
    public int numbers;
}

public static class GameState
{
    public static double Coins;
    public static float CurrentTime;
    public static int CurrentLvl;
    public static int CurrentXP;
    public static UpgradeSave[] boughtUpgrades;

}
