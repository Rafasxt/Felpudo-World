using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerPlataformaController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 7f;
    public float jumpForce = 14f;

    [Header("Dano de Queda")]
    [Tooltip("Altura mínima da queda para causar dano")]
    public float fallDamageMinDistance = 5f;
    [Tooltip("Quanto de vida perde ao cair de muito alto")]
    public int fallDamageAmount = 1;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private float inputX;
    private bool isGrounded;
    private bool wasGrounded;
    private float fallStartY;

    private FelpudoHUD hud;   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        
        hud = FindObjectOfType<FelpudoHUD>();
    }

    private void Update()
    {
        
        inputX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            inputX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            inputX = 1f;

        
        if (inputX > 0.01f)
            sr.flipX = false;     
        else if (inputX < -0.01f)
            sr.flipX = true;      

        // --- PULO ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        
        ChecarDanoDeQueda();

        AtualizarAnimacao();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
    }

    private void AtualizarAnimacao()
    {
        if (anim == null) return;

        float speed = Mathf.Abs(rb.linearVelocity.x);
        anim.SetFloat("Speed", speed);
        anim.SetBool("IsGrounded", isGrounded);
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        
        isGrounded = false;
    }

    
    private void ChecarDanoDeQueda()
    {
        
        if (!isGrounded && wasGrounded)
        {
            fallStartY = transform.position.y;
        }

        
        if (isGrounded && !wasGrounded)
        {
            float distanciaQueda = fallStartY - transform.position.y;

            if (distanciaQueda >= fallDamageMinDistance)
            {
                AplicarDanoDeQueda(distanciaQueda);
            }
        }

        
        wasGrounded = isGrounded;
    }

    private void AplicarDanoDeQueda(float distanciaQueda)
    {
        Debug.Log("Dano de queda! Distância: " + distanciaQueda);

        if (hud != null)
        {
            hud.PerderVida(fallDamageAmount);
        }

        
    }
}
