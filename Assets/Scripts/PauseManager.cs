using UnityEngine;
using UnityEngine.InputSystem; // <--- IMPORTANT

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public static bool isPaused { get; private set; } = false;

    // Référence vers la classe générée par l'Input System
    private Input_Actions_System inputs;

    void Awake()
    {
        // On instancie la classe d'inputs
        inputs = new Input_Actions_System();
    }

    void OnEnable()
    {
        // On s'abonne à l'action "SetPause ON/OFF"
        inputs.Pause.SetPauseONOFF.performed += OnPausePerformed;
        inputs.Enable();
    }

    void OnDisable()
    {
        // On se désabonne proprement
        inputs.Pause.SetPauseONOFF.performed -= OnPausePerformed;
        inputs.Disable();
    }

    void Start()
    {
        // On commence avec le jeu en mode "repris"
        Resume();
    }

    // Callback quand l'action Pause est déclenchée (Échap)
    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SaveManager.I.Save("pause_menu_quitting");
        Application.Quit();
#endif
    }

    public void SaveGame()
    {
        SaveManager.I.Save("pause_menu_manual");
    }
}
