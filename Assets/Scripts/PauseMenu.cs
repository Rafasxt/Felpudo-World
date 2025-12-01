using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("Referências")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Configurações")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private bool manageCursor = false; 

    private void Awake()
    {
        
        Time.timeScale = 1f;
        AudioListener.pause = false;
        IsPaused = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        else
            Debug.LogWarning("[PauseMenu] pauseMenu não foi atribuído no Inspector.");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    
    public void TogglePause()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        if (IsPaused) return;

        IsPaused = true;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        Time.timeScale = 0f;          
        AudioListener.pause = true;   

        if (manageCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Resume()
    {
        if (!IsPaused) return;

        IsPaused = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (manageCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Home()
    {
        RestoreTimeAndAudio();
        SceneManager.LoadScene("MenuInicial");
    }

    public void Restart()
    {
        RestoreTimeAndAudio();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RestoreTimeAndAudio()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        IsPaused = false;

        if (manageCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
