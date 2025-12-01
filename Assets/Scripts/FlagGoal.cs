using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagGoal : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Fase2";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
