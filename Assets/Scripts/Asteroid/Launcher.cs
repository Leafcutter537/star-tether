using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour, IPointerClickHandler
{

    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject launchPoint;
    [SerializeField] private float launchVelocity;
    [SerializeField] private AsteroidFiredEvent asteroidFiredEvent;
    [Header("Gilded Transformation")]
    [SerializeField] private GameObject defaultVisualsPrefab;
    [SerializeField] private GameObject gildedVisualsPrefab;
    [SerializeField] private GameObject currentVisuals;
    [SerializeField] private GildedLauncherOnEvent gildedLauncherOnEvent;
    [SerializeField] private GildedLauncherOffEvent gildedLauncherOffEvent;

    [Header("Scene Objects")]
    public GameObject targetObject;

    private ParticleSystem launchEffect;

    private StopStartController stopStartController;

    private void Awake()
    {
        launchEffect = GetComponentInChildren<ParticleSystem>();
        stopStartController = FindObjectOfType<StopStartController>();
        if (PlayerPrefs.GetInt("GildedLauncherOn", 0) == 1)
            ChangeVisuals(gildedVisualsPrefab);
    }
    private void OnEnable()
    {
        gildedLauncherOnEvent.AddListener(OnGildedLauncherOn);
        gildedLauncherOffEvent.AddListener(OnGildedLauncherOff);
    }
    private void OnDisable()
    {
        gildedLauncherOnEvent.RemoveListener(OnGildedLauncherOn);
        gildedLauncherOffEvent.RemoveListener(OnGildedLauncherOff);
    }
    public void Launch()
    {
        GameObject projectileGameObject = Instantiate(projectilePrefab, launchPoint.transform.position, Quaternion.identity);
        Asteroid projectile = projectileGameObject.GetComponent<Asteroid>();
        Vector2 targetDirection = new Vector2(targetObject.transform.position.x - transform.position.x,
            targetObject.transform.position.y - transform.position.y);
        float targetAngle = Vector2.SignedAngle(Vector2.right, targetDirection);
        projectile.Initialize(targetAngle);
        projectile.rigidbody.AddForce(targetDirection.normalized * launchVelocity, ForceMode2D.Impulse);
        projectile.speed = launchVelocity;
        launchEffect.Play();
        asteroidFiredEvent.Raise(this, null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        stopStartController.Activate();
    }

    private void OnGildedLauncherOn(object sender, EventParameters args)
    {
        ChangeVisuals(gildedVisualsPrefab);
    }
    private void OnGildedLauncherOff(object sender, EventParameters args)
    {
        ChangeVisuals(defaultVisualsPrefab);
    }

    private void ChangeVisuals(GameObject newVisualsPrefab)
    {
        GameObject visualsTemp = currentVisuals;
        currentVisuals = Instantiate(newVisualsPrefab, currentVisuals.transform.position, currentVisuals.transform.rotation);
        currentVisuals.transform.SetParent(transform);
        currentVisuals.transform.localScale = Vector3.one;  
        Destroy(visualsTemp);
    }
}
