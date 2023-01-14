using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit
{

    public float angularVelocity;
    public float perpendicularVelocity;
    public float phi;
    public float radius;

    public Orbit(float perpendicularVelocity, float phi, float radius)
    {
        this.perpendicularVelocity = perpendicularVelocity;
        this.phi = phi;
        this.radius = radius;
        angularVelocity = perpendicularVelocity / radius;
    }

    public void TimeStep(float time)
    {
        phi += time * angularVelocity;
    }

    public Vector2 GetPositionRelativeToTether()
    {
        return new Vector2(radius * Mathf.Cos(phi), radius * Mathf.Sin(phi));
    }

    public Vector2 GetReleaseVelocity()
    {
        float magnitude = angularVelocity * radius;
        float x = Mathf.Cos(phi);
        float y = Mathf.Sin(phi);
        Vector2 directionVector = Vector2.Perpendicular(new Vector2(x, y));
        return magnitude * directionVector;
    }

}
