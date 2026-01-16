using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText;      // Le texte à mettre à jour

    public float currentTime = 0f;

    void Start()
    {
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        UpdateUI();
    }

    public void UpdateUI()
    {
        // Format mm:ss
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
