using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartSpawnerUI : MonoBehaviour
{
    [Header("Referências")]
    public RectTransform canvasRoot; 
    public Sprite heartSprite;       

    [Header("Spawn")]
    public float spawnEvery = 0.08f;
    public Vector2 xPadding = new Vector2(40f, 40f); 
    public Vector2 sizeRange = new Vector2(28f, 60f);
    public int burstOnStart = 10;

    bool spawning;

    public void StartSpawning()
    {
        if (spawning) return;
        spawning = true;

        
        for (int i = 0; i < burstOnStart; i++) SpawnOne();

        
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (spawning)
        {
            SpawnOne();
            yield return new WaitForSeconds(spawnEvery);
        }
    }

    void SpawnOne()
    {
        if (canvasRoot == null || heartSprite == null) return;

        
        GameObject go = new GameObject("HeartUI", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(transform, false); 
        Image img = go.GetComponent<Image>();
        img.sprite = heartSprite;
        img.raycastTarget = false;

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);

        
        float w = canvasRoot.rect.width;
        float h = canvasRoot.rect.height;
        float x = Random.Range(xPadding.x, w - xPadding.y);
        float y = -20f; 

        rt.anchoredPosition = new Vector2(x, y);
        float size = Random.Range(sizeRange.x, sizeRange.y);
        rt.sizeDelta = new Vector2(size, size);

        
        var hf = go.AddComponent<HeartFloat>();
        hf.distanceUp = Random.Range(h * 0.45f, h * 0.75f);
        hf.duration = Random.Range(1.4f, 2.2f);
        hf.rotateZ = Random.Range(-30f, 30f);
        hf.fadeOut = true;
    }
}
