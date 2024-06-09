using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GravityManager : MonoBehaviour
{
    public List<IAttractor> attractors { get; set; }
    public List<IAttractee> attractees { get; set; }

    public float gravitationalConstant = 1f; // Gravitational constant
    public float softeningLength = 1f; // Softening length to prevent excessively high forces
    public GameObject cBodyPrefab; // Prefab for creating new bodies

    private void Start()
    {
        // Find all objects with the Gravity component at the start of the simulation
        var bodies = FindObjectsOfType<MonoBehaviour>();
        attractees = bodies.OfType<IAttractee>().ToList();
        attractors = bodies.OfType<IAttractor>().ToList();
    }

    private void FixedUpdate()
    {
        // Reset accelerations for the next frame
        foreach (var body in attractees)
        {
            body.ResetAcceleration();
        }

        // Apply gravitational forces between each pair of objects
        for (int i = 0; i < attractors.Count; i++)
        {
            for (int j = 0; j < attractees.Count; j++)
            {
                if (attractors[i] != attractees[j]) ApplyGravity(attractors[i], attractees[j]);
            }
        }

        // Update velocities based on the calculated accelerations
        foreach (var body in attractees)
        {
            body.UpdateVelocity(Time.fixedDeltaTime);
        }

        // Update positions based on the new velocities
        foreach (var body in attractees)
        {
            body.UpdatePosition(Time.fixedDeltaTime);
        }
    }

    void ApplyGravity(IAttractor attractor, IAttractee attractee)
    {
        Vector2 direction = attractor.GetPosition() - attractee.GetPosition();
        float distance = direction.magnitude;
        if (distance == 0f) return;
        float softeningDistance = Mathf.Max(distance, softeningLength);

        float forceMagnitude = gravitationalConstant * (attractor.mass * attractee.mass) / (softeningDistance * softeningDistance);
        Vector2 force = direction.normalized * forceMagnitude;

        attractee.ApplyForce(force);
    }

    public void AddAttractor(IAttractor attractor)
    {
        if (!attractors.Contains(attractor)) attractors.Add(attractor);
    }
    public void AddAttractee(IAttractee attractee)
    {
        if (!attractees.Contains(attractee)) attractees.Add(attractee);
    }

    public void MergeBodies(CBody body1, CBody body2)
    {
        float newMass = body1.mass + body2.mass;
        //Debug.Log(newMass);
        Vector2 newVelocity = (body1.velocity * body1.mass + body2.velocity * body2.mass) / newMass; // vel=(m1*v1+m2*v2)/(m1+m2)
        Vector3 newPosition = (body1.GetPosition() + body2.GetPosition()) / 2;  // interpolation of 2 points

        GameObject newBodyObject = Instantiate(cBodyPrefab, newPosition, Quaternion.identity);

        CBody newBody = newBodyObject.GetComponent<CBody>();

        float newRadius = Mathf.Log(newMass)+0.3f;
        newBody.Initialize(newMass, newVelocity, newRadius);

        ColorHandler colorHandler = newBodyObject.GetComponent<ColorHandler>();
        colorHandler.Initialize(newMass);

        // Remove the destroyed bodies from the list
        attractees.Remove(body1);
        attractees.Remove(body2);
        attractors.Remove(body1);
        attractors.Remove(body2);

        body1.Destory();
        body2.Destory();

        // Add the new body to the list
        attractees.Add(newBody);
        attractors.Add(newBody);
    }

}