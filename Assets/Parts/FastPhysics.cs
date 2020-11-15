using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AsteroidsGame;

public class FastPhysics : Part
{
    public static float TimeScale = 1;

    public Vector2 Velocity;
    public float Angularvelocity = 0;
    public float Drag = 0;
    public float Radius = 1;

    public FastPhysics(float drag = 0, float radius = 1)
    {
        Drag = drag;
        Radius = radius;
    }

    public override void Update()
    {
        if (TimeScale > 0)
        {
            if (Drag != 0)
                Velocity *= 1 - Time.deltaTime * Drag;
            angle += Angularvelocity;
        }
        position += (Velocity * Time.deltaTime) * TimeScale;

        //Borders
        Vector2 t = position;
        if (t.x > HalfScreenSize.x && Velocity.x > 0)
            t.x -= HalfScreenSize.x * 2;
        if (t.x < -HalfScreenSize.x && Velocity.x < 0)
            t.x += HalfScreenSize.x * 2;
        if (t.y > HalfScreenSize.y && Velocity.y > 0)
            t.y -= HalfScreenSize.y * 2;
        if (t.y < -HalfScreenSize.y && Velocity.y < 0)
            t.y += HalfScreenSize.y * 2;
        position = t;
    }
    public override void OnDrawGizmos()
    {
        DrawCircle(position, Radius);
    }
    public void AddForce(Vector2 force)
    {
        Velocity += force;
    }
    public void AddAngularForce(float force)
    {
        Angularvelocity += force;
    }
    public bool Equals(MonoBehaviour other)
    {
        return GetHashCode() == other.GetHashCode();
    }
}
