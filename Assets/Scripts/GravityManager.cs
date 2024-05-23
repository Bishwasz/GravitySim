using UnityEngine;
using System.Collections.Generic;

public class GravityManager : MonoBehaviour
{
    private List<CBody> bodies; // List of all objects with gravity

    public float gravitationalConstant = 1f; // Gravitational constant

    private void Start()
    {
        // Find all objects with the Gravity component at the start of the simulation
        bodies = new List<CBody>(FindObjectsOfType<CBody>()); // Added this line
    }

    private void FixedUpdate()
    {
        // Apply gravitational forces between each pair of objects
        for (int i = 0; i < bodies.Count; i++)
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                ApplyGravity(bodies[i], bodies[j]);
            }
        }
    }

    void ApplyGravity(CBody body1, CBody body2)
    {
        Vector2 direction = body2.transform.position - body1.transform.position;
        float distance = direction.magnitude;
        if (distance == 0f) return;

        float forceMagnitude = gravitationalConstant * (body1.mass * body2.mass) / (distance * distance);
        Vector2 force = direction.normalized * forceMagnitude;

        body1.ApplyForce(force);
        body2.ApplyForce(-force); // Newton's third law
    }
    public void AddCBody(CBody newCBody)
    {
        if (!bodies.Contains(newCBody))
        {
            bodies.Add(newCBody);
        }
    }
}