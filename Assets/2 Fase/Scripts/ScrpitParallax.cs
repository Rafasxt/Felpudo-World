using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Material mat;
    private float distance;

    [Range(0f, 5f)]
    public float speed = 0.5f;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        
        float mult = (GameManager.Instance != null) ? GameManager.Instance.globalSpeed : 1f;

        distance += Time.deltaTime * speed * mult;

        
        mat.SetTextureOffset("_MainTex", Vector2.right * distance);
    }
}
