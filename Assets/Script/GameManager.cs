using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    [Header("Pause")]
    public GameObject pauseMenuUI;
    public InputActionReference InputPause;
    public bool IsPaused { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        pauseMenuUI.SetActive(false);
    }

    private void OnEnable()
    {
        InputPause.action.performed += MenuPause;
    }

    private void OnDisable()
    {
        InputPause.action.performed -= MenuPause;
    }

    public void MenuPause(InputAction.CallbackContext _ctx)
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            AudioListener.pause = false;
        }
    }
}
