using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorHandler : MonoBehaviour
{
    Color bodyColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private SpriteRenderer spriteRenderer;


    public void Initialize(float mass)
    {
        float m = 1f - (1.0f / (1.0f + Mathf.Exp(-mass + 2)));
        float red = 1 / (Mathf.Exp(0.05f * mass));
        float green = 1 / (Mathf.Exp(0.5f * mass));
        float blue = mass / (mass + 1);
        bodyColor = new Color(m, green, blue, 1f);
        UpdateColor();


    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();

    }

    private void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = bodyColor;
        }
    }
}