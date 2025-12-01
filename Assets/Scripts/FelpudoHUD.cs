using UnityEngine;
using UnityEngine.UI;

public class FelpudoHUD : MonoBehaviour
{
    [Header("Vida (HUD redondo)")]
    public Image vidaImage;          
    public Sprite[] estadosVida;     

    [Header("Penas Douradas")]
    public Text textoPena;           

    private int indiceVidaAtual;     
    private int penasAtuais = 0;

    private void Start()
    {
        
        if (estadosVida != null && estadosVida.Length > 0)
        {
            indiceVidaAtual = estadosVida.Length - 1;
        }
        AtualizarHUD();
    }

    
    public void PerderVida(int quantidade = 1)
    {
        indiceVidaAtual -= quantidade;
        indiceVidaAtual = Mathf.Clamp(indiceVidaAtual, 0, estadosVida.Length - 1);
        AtualizarHUD();
    }

    
    public void GanharVida(int quantidade = 1)
    {
        indiceVidaAtual += quantidade;
        indiceVidaAtual = Mathf.Clamp(indiceVidaAtual, 0, estadosVida.Length - 1);
        AtualizarHUD();
    }

    
    public void AdicionarPena(int quantidade)
    {
        penasAtuais += quantidade;
        AtualizarHUD();
    }

    private void AtualizarHUD()
    {
        
        if (vidaImage != null && estadosVida != null && estadosVida.Length > 0)
        {
            vidaImage.sprite = estadosVida[indiceVidaAtual];
        }

        
        if (textoPena != null)
        {
            textoPena.text = "x" + penasAtuais.ToString();
        }
    }
}
