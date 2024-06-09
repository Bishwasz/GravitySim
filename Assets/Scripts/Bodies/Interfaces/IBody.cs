using UnityEngine;

public interface IBody
{
    public float mass { get; set; }
    public Vector2 GetPosition();
    public void Destory();
}
