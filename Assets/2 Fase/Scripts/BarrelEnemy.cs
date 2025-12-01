using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BarrelEnemy : MonoBehaviour
{
    [Header("Configurações do Barril")]
    public float rollSpeed = 3.5f;         
    public int damage = 1;                 
    public string groundLayerName = "Ground";
    public float destroyX = -10f;          

    private Rigidbody2D rb;
    private Animator anim;
    private bool hasTouchedGround = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 2f;
        rb.freezeRotation = true;

        
        if (anim != null)
            anim.enabled = false;
    }

    void FixedUpdate()
    {
        
        if (hasTouchedGround)
        {
            float mult = (GameManager.Instance != null) ? GameManager.Instance.globalSpeed : 1f;
            rb.linearVelocity = new Vector2(-rollSpeed * mult, rb.linearVelocity.y);
        }

        
        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (!hasTouchedGround && collision.collider.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            hasTouchedGround = true;
            if (anim != null)
                anim.enabled = true; 
        }

        
        if (collision.collider.CompareTag("Player"))
        {
            
            float playerY = collision.collider.transform.position.y;
            float enemyY = transform.position.y;

            if (playerY > enemyY + 0.3f)
            {
                
                Destroy(gameObject);
            }
            else
            {
                
                PlayerHealth ph = collision.collider.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(damage);

                Destroy(gameObject);
            }
        }
    }
}
