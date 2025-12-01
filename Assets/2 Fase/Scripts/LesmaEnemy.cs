using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LesmaEnemy : MonoBehaviour
{
    [Header("Hitboxes (arraste os filhos)")]
    public Collider2D hitTop;   
    public Collider2D hitBody;  

    [Header("Movimento")]
    public float moveSpeed = 3.5f;
    public float destroyX = -10f;

    [Header("Dano / Stomp")]
    public int damage = 1;
    public float minDownSpeed = -0.05f;       
    public float verticalSlack = 0.02f;       
    public float stompHorizontalPadding = 0.25f; 
    public float stompBounce = 7f;

    [Header("Debug")]
    public bool debugLogs = false;

    Rigidbody2D rb;
    bool resolved; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (hitTop != null) hitTop.isTrigger = true;
        if (hitBody != null) hitBody.isTrigger = true;
    }

    void OnEnable()
    {
        if (hitTop) HitboxForwarder.Bind(hitTop, OnHitTop);
        if (hitBody) HitboxForwarder.Bind(hitBody, OnHitBody);
    }
    void OnDisable()
    {
        if (hitTop) HitboxForwarder.Unbind(hitTop, OnHitTop);
        if (hitBody) HitboxForwarder.Unbind(hitBody, OnHitBody);
    }

    void Update()
    {
        if (resolved) return;

        float mult = (GameManager.Instance != null) ? GameManager.Instance.globalSpeed : 1f;
        transform.Translate(Vector2.left * moveSpeed * mult * Time.deltaTime);

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }

    bool IsStomp(Collider2D playerCol)
    {
        if (!playerCol.CompareTag("Player")) return false;

        
        var prb = playerCol.attachedRigidbody;
        float vy = (prb ? prb.linearVelocity.y : 0f);
        if (vy > minDownSpeed) return false;

        
        Bounds eb = hitBody ? hitBody.bounds : GetComponent<Collider2D>().bounds;
        float playerBottom = playerCol.bounds.min.y;
        float midY = eb.center.y + verticalSlack;
        bool verticallyAbove = playerBottom > midY;
        if (!verticallyAbove) return false;

        
        float px = playerCol.bounds.center.x;
        bool horizontallyOver = (px >= eb.min.x - stompHorizontalPadding && px <= eb.max.x + stompHorizontalPadding);
        return horizontallyOver;
    }

    void ResolveStomp(Rigidbody2D playerRb)
    {
        if (resolved) return;
        resolved = true;

        if (hitTop) hitTop.enabled = false;
        if (hitBody) hitBody.enabled = false;

        if (playerRb) playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounce);

        if (debugLogs) Debug.Log("[LESMA] STOMP (lesma destruída).");
        Destroy(gameObject);
    }

    void ResolveDamage(GameObject player)
    {
        if (resolved) return;
        resolved = true;

        var ph = player.GetComponent<PlayerHealth>();
        if (ph != null) ph.TakeDamage(damage);

        if (hitTop) hitTop.enabled = false;
        if (hitBody) hitBody.enabled = false;

        if (debugLogs) Debug.Log("[LESMA] DANO aplicado e lesma destruída.");
        Destroy(gameObject);
    }

    void OnHitTop(Collider2D other)
    {
        if (resolved || !other.CompareTag("Player")) return;
        if (IsStomp(other)) ResolveStomp(other.attachedRigidbody);
        
    }

    void OnHitBody(Collider2D other)
    {
        if (resolved || !other.CompareTag("Player")) return;

        
        if (IsStomp(other)) { ResolveStomp(other.attachedRigidbody); return; }

       
        ResolveDamage(other.gameObject);
    }
}


public class HitboxForwarder : MonoBehaviour
{
    public delegate void HitCallback(Collider2D other);
    HitCallback cb;

    public static void Bind(Collider2D col, HitCallback onHit)
    {
        var f = col.GetComponent<HitboxForwarder>();
        if (f == null) f = col.gameObject.AddComponent<HitboxForwarder>();
        f.cb = onHit;
    }
    public static void Unbind(Collider2D col, HitCallback onHit)
    {
        var f = col.GetComponent<HitboxForwarder>();
        if (f != null) f.cb = null;
    }

    void OnTriggerEnter2D(Collider2D other) => cb?.Invoke(other);
}
