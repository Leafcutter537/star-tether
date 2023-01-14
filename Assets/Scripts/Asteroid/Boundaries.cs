using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    [SerializeField] AsteroidDestroyedEvent asteroidDestroyedEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            asteroidDestroyedEvent.Raise(this, null);
            Destroy(collision.gameObject);
        }
    }
}
