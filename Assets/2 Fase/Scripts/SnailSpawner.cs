using UnityEngine;

public class SnailSpawner : MonoBehaviour
{
    public GameObject snailPrefab;

    [Header("Tempo")]
    public float minSpawnTime = 2.4f;
    public float maxSpawnTime = 4.4f;

    [Header("Chão")]
    public float yGround = -4.0f;

    [Header("Anti-grude")]
    public float separationRadius = 2.4f;
    public LayerMask enemyLayer;

    float timer;

    void Start()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.IsGameEnded()) return;
            if (GameManager.Instance.IsEndingPhase) return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            TrySpawn();
            timer = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    void TrySpawn()
    {
        if (!snailPrefab) return;

        Vector3 pos = transform.position;
        pos.y = yGround;


        if (Physics2D.OverlapCircle(pos, separationRadius, enemyLayer) != null) return;

        Instantiate(snailPrefab, pos, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var p = transform.position; p.y = yGround;
        Gizmos.DrawWireSphere(p, separationRadius);
    }
}