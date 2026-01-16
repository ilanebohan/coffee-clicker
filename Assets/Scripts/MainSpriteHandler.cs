using UnityEngine;
using UnityEngine.InputSystem;

public class MainSpriteHandler : MonoBehaviour
{
    public GameManager gameManager;

    private Camera cam;

    // Clicked animation
    private Animator anim;

    void Awake()
    {
        cam = Camera.main; // assure que ta caméra est taggée "MainCamera"
        anim = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Coffee Object Created");
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.isPaused)
            return;

        checkMouseClick();
    }

    void TryClickAtScreenPos(Vector2 screenPos)
    {
        if (cam == null) return;

        // Orthographic : le z n’a pas d’importance, on met 0
        Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));

        // Teste ce qui est sous le pointeur
        Collider2D hit = Physics2D.OverlapPoint(world);
        if (hit != null && hit.transform == transform)
        {
            Debug.Log("Mouse Click");
            gameManager.AddScore();

            if (anim != null)
                anim.SetTrigger("Clicked");
        }
    }

    void checkMouseClick()
    {
        // Souris
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screen = Mouse.current.position.ReadValue();
            TryClickAtScreenPos(screen);
        }

        // Tactile
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 screen = Touchscreen.current.primaryTouch.position.ReadValue();
            TryClickAtScreenPos(screen);
        }
    }
}
