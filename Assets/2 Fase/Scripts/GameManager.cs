using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameDuration = 120f;

    public Text txtTempo;

    public Image[] heartImages;
    public Image[] shieldImages;

    public GameObject winPanel;
    public GameObject losePanel;

    public string finalSceneName = "EncontrotFinal";

    public float globalSpeed = 1f;

    private float currentTime;
    private bool gameEnded = false;
    private bool endingPhaseStarted = false;

    public bool IsEndingPhase => !gameEnded && currentTime <= 3f;
    public bool IsGameEnded() => gameEnded;

    private Coroutine speedBoostRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        currentTime = gameDuration;

        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);

        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph) UpdateHUD(ph);

        UpdateTimerUI();
    }

    void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime;
        if (currentTime < 0f) currentTime = 0f;

        UpdateTimerUI();

        if (!endingPhaseStarted && currentTime <= 3f)
            BeginEndingPhase();

        if (currentTime <= 0f)
            Win();
    }

    private void UpdateTimerUI()
    {
        if (!txtTempo) return;
        int t = Mathf.CeilToInt(currentTime);
        txtTempo.text = $"{t / 60:00}:{t % 60:00}";
    }

    public void UpdateHUD()
    {
        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph) UpdateHUD(ph);
    }

    public void UpdateHUD(PlayerHealth player)
    {
        if (heartImages != null)
            for (int i = 0; i < heartImages.Length; i++)
                if (heartImages[i]) heartImages[i].enabled = i < player.currentHealth;

        if (shieldImages != null)
            for (int i = 0; i < shieldImages.Length; i++)
                if (shieldImages[i]) shieldImages[i].enabled = i < player.currentShields;
    }

    private void BeginEndingPhase()
    {
        endingPhaseStarted = true;

        foreach (var mb in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (mb == null) continue;
            string n = mb.GetType().Name;
            if (n == "EnemySpawner" || n == "SnailSpawner" || n == "FruitSpawner")
            {
                var b = mb as Behaviour;
                if (b) b.enabled = false;
            }
        }

        SafeDestroyByTag("Enemy");
        SafeDestroyByTag("Fruit");
    }

    private void SafeDestroyByTag(string tag)
    {
        try
        {
            var objs = GameObject.FindGameObjectsWithTag(tag);
            foreach (var o in objs) if (o) Destroy(o);
        }
        catch { }
    }

    public void ApplyGlobalSpeedMultiplier(float multiplier, float duration)
    {
        if (speedBoostRoutine != null) StopCoroutine(speedBoostRoutine);
        speedBoostRoutine = StartCoroutine(CoSpeed(multiplier, duration));
    }

    IEnumerator CoSpeed(float mult, float dur)
    {
        float original = globalSpeed;
        globalSpeed = mult;
        float t = 0f;
        while (t < dur && !gameEnded) { t += Time.deltaTime; yield return null; }
        globalSpeed = original;
        speedBoostRoutine = null;
    }

    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 0f;
        if (losePanel) losePanel.SetActive(true);
    }

    public void Win()
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(finalSceneName))
        {
            SceneManager.LoadScene(finalSceneName);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float GetTimeLeft() => currentTime;
}
