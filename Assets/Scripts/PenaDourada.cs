using UnityEngine;

public class PenaDourada : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        
        FelpudoHUD hud = FindObjectOfType<FelpudoHUD>();
        if (hud != null)
        {
            hud.AdicionarPena(1);
        }

        
        Destroy(gameObject);
    }
}
