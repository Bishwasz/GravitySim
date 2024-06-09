using System;
using UnityEngine;

public interface IAttractee : IBody
{
    public Vector2 velocity { get; set; }
    public void ResetAcceleration();
    public void ApplyForce(Vector2 force);
    public void UpdateVelocity(float deltaTime);
    public void UpdatePosition(float deltaTime);
}
