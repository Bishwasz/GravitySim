using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryHandler : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ConfigureLineRenderer();
    }

    private void ConfigureLineRenderer()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = new Color(1, 1, 0, 1); // yellow
        lineRenderer.endColor = new Color(1, 1, 1, 0); // Fades to transparent
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    public void DrawTrajectory(Vector2 startPosition, float G, Vector2 initialVelocity, List<IAttractor> attractors, float timeStep, int steps)
    {
        lineRenderer.positionCount = steps;
        Vector2 currentPos = startPosition;
        Vector2 currentVel = initialVelocity;

        for (int i = 0; i < steps; i++)
        {
            Vector2 acceleration = Vector2.zero;

            foreach (var other in attractors)
            {
                Vector2 direction = other.GetPosition() - currentPos;
                float distance = direction.magnitude;
                // TODO: handle collisions

                float accMagnitude = G * other.mass / (distance * distance);
                Vector2 acc = direction.normalized * accMagnitude;
                acceleration += acc;
            }

            currentVel += acceleration * timeStep;
            currentPos += currentVel * timeStep;
            lineRenderer.SetPosition(i, currentPos);
        }
    }

    public void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }

    public void EnableTrajectory()
    {
        lineRenderer.enabled = true;
    }

    public void DisableTrajectory()
    {
        lineRenderer.enabled = false;
    }
}