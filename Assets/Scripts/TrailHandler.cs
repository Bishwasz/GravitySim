using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailHandler : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        ConfigureTrailRenderer();
    }

    private void ConfigureTrailRenderer()
    {
        trailRenderer.time = 2.0f; // Trail duration
        trailRenderer.startWidth = 0.1f;
        trailRenderer.endWidth = 0.1f;
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.startColor = Color.white;
        trailRenderer.endColor = new Color(1, 1, 1, 0); // Fades to transparent
        trailRenderer.minVertexDistance = 0.1f;
    }

    public void EnableTrail()
    {
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }
    }

    public void DisableTrail()
    {
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }
}