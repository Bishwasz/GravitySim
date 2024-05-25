using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CBody : MonoBehaviour
{
    public float mass = 1f; // Mass of the object
    public float radius = 6f;
    public Vector2 velocity = Vector2.zero; // Initial velocity
    public Vector2 acceleration = Vector2.zero; // Acceleration (calculated from forces)

    private Rigidbody2D rb; // this body

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass; // Ensure the Rigidbody2D mass matches the Gravity mass
    }

    // Reset the acceleration for the next frame
    public void ResetAcceleration()
    {
        acceleration = Vector2.zero;
    }

    // Apply a force to the object
    public void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    }

    // Update the velocity based on acceleration
    public void UpdateVelocity(float deltaTime)
    {
        velocity += acceleration * deltaTime;
    }

    // Update the position based on velocity
    public void UpdatePosition(float deltaTime)
    {
        Vector2 newPosition = rb.position + velocity * deltaTime;
        rb.MovePosition(newPosition);
    }

    private void UpdateScale()
    {
        transform.localScale = new Vector3(radius, radius, 1f); // Assuming the original scale is 1 unit in diameter
    }

    //// Method to change the radius
    //public void SetRadius(float newRadius)
    //{
    //    radius = newRadius;
    //    UpdateScale();
    //}
}