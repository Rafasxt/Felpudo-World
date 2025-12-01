using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class LinhaDeDialogo
{
    public string personagem; 
    public Sprite retrato;   
    [TextArea]
    public string texto;      
}

public class DialogueCutscene : MonoBehaviour
{
    [Header("Configuração do Diálogo")]
    public LinhaDeDialogo[] falas;
    public float velocidadeTexto = 0.04f;

    [Header("Referências do HUD")]
    public GameObject dialogBox;
    public RectTransform portraitRect; 
    public Image portraitImage;
    public Text dialogueText;
    public Text continueText;

    [Header("Posições dos retratos")]
    public Vector2 posicaoEsquerda = new Vector2(-300, 80);
    public Vector2 posicaoDireita = new Vector2(300, 80);

    private int index = 0;
    private bool escrevendo = false;
    private bool dialogoAtivo = true;
    private GameObject jogador;

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null) jogador.SetActive(false); 

        Time.timeScale = 0f;
        continueText.gameObject.SetActive(false);
        dialogBox.SetActive(true);

        MostrarFala();
    }

    private void Update()
    {
        if (dialogoAtivo && Input.GetKeyDown(KeyCode.Space))
        {
            if (escrevendo)
            {
                StopAllCoroutines();
                dialogueText.text = falas[index].texto;
                escrevendo = false;
                continueText.gameObject.SetActive(true);
            }
            else
            {
                continueText.gameObject.SetActive(false);
                index++;
                MostrarFala();
            }
        }
    }

    void MostrarFala()
    {
        if (index >= falas.Length)
        {
            FinalizarDialogo();
            return;
        }

        portraitImage.sprite = falas[index].retrato;

        
        if (falas[index].personagem.ToLower().Contains("uruca"))
            portraitRect.anchoredPosition = posicaoDireita;
        else
            portraitRect.anchoredPosition = posicaoEsquerda;

        StopAllCoroutines();
        StartCoroutine(EscreverTexto());
    }

    IEnumerator EscreverTexto()
    {
        escrevendo = true;
        dialogueText.text = "";

        foreach (char letra in falas[index].texto)
        {
            dialogueText.text += letra;
            yield return new WaitForSecondsRealtime(velocidadeTexto);
        }

        escrevendo = false;
        continueText.gameObject.SetActive(true);
    }

    void FinalizarDialogo()
    {
        dialogoAtivo = false;
        dialogBox.SetActive(false);
        Time.timeScale = 1f;

        if (jogador != null) jogador.SetActive(true);

        Destroy(gameObject);
    }
}
