using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject launchPoint;

    public GameObject targetObject;
    private ParticleSystem launchEffect;

    private void Awake()
    {
        launchEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void Launch()
    {
        GameObject projectileGameObject = Instantiate(projectilePrefab, launchPoint.transform.position, Quaternion.identity);
        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        Vector2 targetDirection = new Vector2(targetObject.transform.position.x - transform.position.x,
            targetObject.transform.position.y - transform.position.y);
        float targetAngle = Vector2.SignedAngle(Vector2.right, targetDirection);
        projectile.Initialize(targetAngle);
        projectile.rigidbody.AddForce(targetDirection.normalized * projectile.launchVelocity, ForceMode2D.Impulse);
        launchEffect.Play();
    }
}
