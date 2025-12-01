using UnityEngine;

public class PauseWhileDialogueActive : MonoBehaviour
{
    [Header("Raiz do diálogo (objeto que fica ATIVO enquanto o diálogo está na tela)")]
    public GameObject dialogueRoot;

    void Update()
    {
        if (dialogueRoot == null) return;

        
        if (dialogueRoot.activeInHierarchy)
        {
            if (Time.timeScale != 0f)
                Time.timeScale = 0f;
        }
        else
        {
            
            if (Time.timeScale != 1f)
                Time.timeScale = 1f;

            enabled = false;
        }
    }
}
