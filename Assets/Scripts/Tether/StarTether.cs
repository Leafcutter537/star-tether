using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StarTether : MonoBehaviour
{
    private static float lifeTime = 2;
    private float timeLeft;
    private bool isMarkedForDestruction;
    private new ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (isMarkedForDestruction)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
                Destroy(gameObject);
        }
    }


    public void MarkForDestruction()
    {
        particleSystem.Stop();
        isMarkedForDestruction = true;
        timeLeft = lifeTime;
    }


}
