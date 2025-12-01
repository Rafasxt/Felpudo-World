using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject barrelPrefab;       

    [Header("Área de spawn no mundo (X)")]
    public float xMin = 2f;
    public float xMax = 9f;

    [Header("Altura do drop (Y)")]
    public float dropY = 3.5f;

    [Header("Tempo entre spawns")]
    public float minDelay = 2.0f;
    public float maxDelay = 4.0f;

    [Header("Espaçamento")]
    public float minDistanceBetween = 1.5f;   
    public float overlapRadius = 0.4f;        

    [Header("Opcional")]
    public Transform spawnPoint;  

    float timer;
    float nextDelay;
    float lastSpawnX = 9999f;

    void OnEnable()
    {
        ScheduleNext();
    }

    void Update()
    {
        
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.IsGameEnded()) return;
            if (GameManager.Instance.IsEndingPhase) return;
        }

        if (barrelPrefab == null)
            return;

        timer += Time.deltaTime;
        if (timer >= nextDelay)
        {
            TrySpawn();
            ScheduleNext();
        }
    }

    void ScheduleNext()
    {
        timer = 0f;
        nextDelay = Random.Range(minDelay, maxDelay);
    }

    void TrySpawn()
    {
        Transform sp = spawnPoint != null ? spawnPoint : transform;

        
        float x = Random.Range(xMin, xMax);
        if (Mathf.Abs(x - lastSpawnX) < minDistanceBetween)
        {
            
            x += Mathf.Sign(Random.Range(-1f, 1f)) * minDistanceBetween;
            x = Mathf.Clamp(x, Mathf.Min(xMin, xMax), Mathf.Max(xMin, xMax));
        }

        Vector3 pos = new Vector3(x, dropY, sp.position.z);

        
        if (Physics2D.OverlapCircle(pos, overlapRadius, LayerMask.GetMask("Enemy")) != null)
            return;

        GameObject go = Instantiate(barrelPrefab, pos, Quaternion.identity);
        go.tag = "Enemy"; 
        lastSpawnX = x;
    }
}

