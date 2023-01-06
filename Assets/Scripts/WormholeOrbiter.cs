using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeOrbiter : MonoBehaviour
{

    private Target wormhole;
    private Orbit orbit;
    private new Rigidbody2D rigidbody;
    private new ParticleSystem particleSystem;

    public void Initialize()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (wormhole != null)
        {
            orbit.TimeStep(Time.deltaTime);
            Vector2 wormholePosition = new Vector2(wormhole.transform.position.x, wormhole.transform.position.y);
            Vector2 newPosition = wormholePosition + orbit.GetPositionRelativeToTether();
            rigidbody.MovePosition(newPosition);
            Vector2 inwardAngle = (wormholePosition - newPosition).normalized;
            particleSystem.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, inwardAngle));
        }
    }


    public void StartOrbit(Target wormhole, float perpendicularVelocity)
    {
        Vector2 wormholePosition = new Vector2(wormhole.transform.position.x, wormhole.transform.position.y);
        Vector2 wormholeToObject = rigidbody.position - wormholePosition;
        float phi = Mathf.Acos(wormholeToObject.normalized.x);
        if (wormholeToObject.y < 0)
        {
            phi = -1 * phi;
        }
        orbit = new Orbit(perpendicularVelocity, phi, wormholeToObject.magnitude);
        this.wormhole = wormhole;
    }


}
