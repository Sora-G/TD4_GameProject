using UnityEngine;

public class LavaScroll : MonoBehaviour
{
    public float speedX = 0.5f;
    public float speedY = 0.5f;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offsetX = Time.time * speedX;
        float offsetY = Time.time * speedY;

        rend.material.mainTextureOffset =
            new Vector2(offsetX, offsetY);
    }
}
