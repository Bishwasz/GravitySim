using UnityEngine;

public class CBody : MonoBehaviour
{
    public float mass = 1f; // Mass of the object
    public float radius = 6f;
    public Vector2 velocity = Vector2.zero; // Initial velocity
    public Vector2 acceleration = Vector2.zero; // Acceleration (calculated from forces)

    private void Update()
    {
        // Update the position based on velocity
        transform.position += (Vector3)velocity * Time.deltaTime;
        UpdateScale();

        // Reset acceleration for the next frame
        acceleration = Vector2.zero;
    }

    // Apply a force to the object
    public void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    }

    private void FixedUpdate()
    {
        // Update the velocity based on acceleration
        velocity += acceleration * Time.fixedDeltaTime;
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