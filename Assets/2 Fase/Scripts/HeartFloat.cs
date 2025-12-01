using UnityEngine;
using UnityEngine.UI;

public class HeartFloat : MonoBehaviour
{
    public float duration = 1.8f;
    public float distanceUp = 400f;
    public float rotateZ = 15f;
    public bool fadeOut = true;

    float t;
    RectTransform rt;
    Image img;
    Vector2 startPos;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        startPos = rt.anchoredPosition;
    }

    void Update()
    {
        t += Time.deltaTime;
        float k = Mathf.Clamp01(t / duration);

        // sobe
        rt.anchoredPosition = startPos + Vector2.up * (distanceUp * k);

        // gira
        rt.localRotation = Quaternion.Euler(0, 0, rotateZ * k);

        // fade-out
        if (fadeOut && img != null)
        {
            var c = img.color;
            c.a = 1f - k;
            img.color = c;
        }

        if (k >= 1f) Destroy(gameObject);
    }
}
