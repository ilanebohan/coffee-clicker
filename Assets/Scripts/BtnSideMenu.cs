using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnSideMenu : MonoBehaviour
{

    public GameObject sideMenu; // Side menu to show / hide
    public GameObject btn; // Button to toggle the side menu

    private RectTransform btnRect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btnRect = btn.GetComponent<RectTransform>();

        // Position du bouton quand le menu est visible
        btnRect.anchoredPosition = new Vector2(389f, 437f);
        btn.GetComponentInChildren<TextMeshProUGUI>().text = ">";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleMenu()
    {
        if (sideMenu.activeSelf)
        {
            hideMenu();
        }
        else
        {
            showMenu();
        }
    }

    void hideMenu()
    {
        sideMenu.SetActive(false);

        // Position du bouton quand le menu est caché
        btnRect.anchoredPosition = new Vector2(843f, 437f);
        btn.GetComponentInChildren<TextMeshProUGUI>().text = "<";
    }

    void showMenu()
    {
        sideMenu.SetActive(true);

        // Position du bouton quand le menu est visible
        btnRect.anchoredPosition = new Vector2(389f, 437f);
        btn.GetComponentInChildren<TextMeshProUGUI>().text = ">";
    }
}
