using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Attractor : MonoBehaviour, IAttractor
{
    [SerializeField]
    private float _mass;
    public float mass {
        get { return _mass; }
        set { _mass = mass;  }
    }
    public float radius = 0.5f;
    private Rigidbody2D rb;
    private bool hasMerged = false; // Flag to check if the body has merged


    public void Initialize(float newMass, Vector2 newVelocity, float newRadius)
    {
        mass = newMass;
        radius = Mathf.Log(newMass) + 0.3f;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.mass = newMass;
        UpdateScale();
    }

    private void Awake()
    {
        mass = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass; // Ensure the Rigidbody2D mass matches the Gravity mass

    }

    public Vector2 GetPosition()
    {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    private void UpdateScale()
    {
        transform.localScale = new Vector3(radius, radius, 1f); // Assuming the original scale is 1 unit in diameter
    }

    public void Destory()
    {
        Destroy(this.gameObject);
    }
}