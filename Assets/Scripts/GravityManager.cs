using UnityEngine;
using System.Collections.Generic;

public class GravityManager : MonoBehaviour
{
    private List<CBody> bodies; // List of all objects with gravity

    public float gravitationalConstant = 1f; // Gravitational constant
    public float softeningLength = 0.5f; // Softening length to prevent excessively high forces
    public GameObject cBodyPrefab; // Prefab for creating new bodies

    private void Start()
    {
        // Find all objects with the Gravity component at the start of the simulation
        bodies = new List<CBody>(FindObjectsOfType<CBody>());
    }

    private void FixedUpdate()
    {
        // Reset accelerations for the next frame
        foreach (var body in bodies)
        {
            body.ResetAcceleration();
        }

        // Apply gravitational forces between each pair of objects
        for (int i = 0; i < bodies.Count; i++)
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                ApplyGravity(bodies[i], bodies[j]);
            }
        }

        // Update velocities based on the calculated accelerations
        foreach (var body in bodies)
        {
            body.UpdateVelocity(Time.fixedDeltaTime);
        }

        // Update positions based on the new velocities
        foreach (var body in bodies)
        {
            body.UpdatePosition(Time.fixedDeltaTime);
        }
    }

    void ApplyGravity(CBody body1, CBody body2)
    {
        Vector2 direction = body2.transform.position - body1.transform.position;
        float distance = direction.magnitude;
        if (distance == 0f) return;
        float softeningDistance = Mathf.Max(distance, softeningLength);

        float forceMagnitude = gravitationalConstant * (body1.mass * body2.mass) / (softeningDistance * softeningDistance);
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

    public void MergeBodies(CBody body1, CBody body2)
    {
        float newMass = body1.mass + body2.mass;
        Debug.Log(newMass);
        Vector2 newVelocity = (body1.velocity * body1.mass + body2.velocity * body2.mass) / newMass;
        Vector3 newPosition = (body1.transform.position + body2.transform.position) / 2;

        GameObject newBodyObject = Instantiate(cBodyPrefab, newPosition, Quaternion.identity);

        CBody newBody = newBodyObject.GetComponent<CBody>();

        newBody.Initialize(newMass, newVelocity, Mathf.Max(body1.radius, body2.radius));

        Destroy(body1.gameObject);
        Destroy(body2.gameObject);

        // Remove the destroyed bodies from the list
        bodies.Remove(body1);
        bodies.Remove(body2);

        // Add the new body to the list
        AddCBody(newBody);
    }
}