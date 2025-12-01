using UnityEngine;

public enum FruitType
{
    Banana,
    Watermelon
}

[RequireComponent(typeof(Collider2D))]
public class FruitPickup : MonoBehaviour
{
    public FruitType fruitType;
    public float moveSpeed = 2.5f;

    [Header("Banana")]
    public float worldMultiplier = 1.5f;
    public float worldDuration = 3f;

    [Header("Explosão")]
    public Animator anim;
    public string explodeTrigger = "Explode";
    public float destroyDelay = 0.4f;

    private bool picked = false;

    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (picked) return;


        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);


        if (transform.position.x < -12f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (picked) return;
        if (!other.CompareTag("Player")) return;

        picked = true;

        ApplyEffect(other.gameObject);

        if (anim != null && !string.IsNullOrEmpty(explodeTrigger))
            anim.SetTrigger(explodeTrigger);


        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, destroyDelay);
    }

    void ApplyEffect(GameObject player)
    {
        switch (fruitType)
        {
            case FruitType.Banana:

                if (GameManager.Instance != null)
                    GameManager.Instance.ApplyGlobalSpeedMultiplier(1.5f, 5f);
                


                break;

            case FruitType.Watermelon:

                var ph = player.GetComponent<PlayerHealth>();
                if (ph != null)
                {
                    if (ph.currentHealth < ph.maxHealth)
                        ph.Heal(1);
                    else
                        ph.AddShield(1);
                }
                break;
        }
    }
}