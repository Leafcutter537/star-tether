using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField] private float orbiterPerpendicularVelocity;
    [SerializeField] private float orbiterDistance;
    [SerializeField] private float orbiterNumber;
    [SerializeField] private GameObject orbiterPrefab;

    private void Awake()
    {
        for (int i = 0; i < orbiterNumber; i++)
        {
            float angle = (360f / orbiterNumber) * i;
            Vector3 orbiterPosition = new Vector3(transform.position.x + Mathf.Cos(angle) * orbiterDistance,
                transform.position.y + Mathf.Sin(angle) * orbiterDistance,
                0);
            GameObject orbiterObject = Instantiate(orbiterPrefab, orbiterPosition, Quaternion.identity);
            WormholeOrbiter orbiter = orbiterObject.GetComponent<WormholeOrbiter>();
            orbiter.Initialize();
            orbiter.StartOrbit(this, orbiterPerpendicularVelocity);
        }
    }

}
