using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class CBody : MonoBehaviour
{
    public float mass = 0.5f; // Mass of the object
    public float radius = 0.5f;
    public Vector2 velocity = Vector2.zero; // Initial velocity
    public Vector2 acceleration = Vector2.zero; // Acceleration (calculated from forces)
    Color bodyColor=new Color(0.5f,0.5f,0.5f,1f);
    private Rigidbody2D rb;
    private bool hasMerged = false; // Flag to check if the body has merged
    private SpriteRenderer spriteRenderer;


    public void Initialize(float newMass, Vector2 newVelocity, float newRadius)
    {
        mass = newMass;
        velocity = newVelocity;
        radius = Mathf.Log(newMass)+0.3f;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.mass = newMass;
        UpdateScale();

        float m = 1f-(1.0f / (1.0f + Mathf.Exp(-mass+2)));
        float red = 1 / (Mathf.Exp(0.05f * mass));
        float green=1/(Mathf.Exp(0.5f*mass)) ;
        float blue =mass/(mass+1);
        bodyColor = new Color(m, green, blue, 1f);
        UpdateColor();


    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.mass = mass; // Ensure the Rigidbody2D mass matches the Gravity mass
        UpdateColor();

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

    public Vector2 GetPosition()
    {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    private void UpdateScale()
    {
        transform.localScale = new Vector3(radius, radius, 1f); // Assuming the original scale is 1 unit in diameter
    }
    private void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = bodyColor;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CBody otherBody = collision.gameObject.GetComponent<CBody>();
        if (otherBody != null && !hasMerged && !otherBody.hasMerged)
        {
            hasMerged = true; 
            FindObjectOfType<GravityManager>().MergeBodies(this, otherBody);
        }
    }
}