using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class AntimatterAsteroid : Asteroid
{
    [SerializeField] private TetherSwitch initialTether;
    [SerializeField] private Vector2 initialVelocity;
    [Header("Antimatter Time Dilation")]
    [SerializeField] private float antimatterTimeFactor;
    [SerializeField] private SlowAntimatterEvent slowAntimatterEvent;
    [SerializeField] private Animator animator;
    private bool isSlowed;

    protected override void Awake()
    {
        base.Awake();
        rigidbody.velocity = initialVelocity;
        particleSystem.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, initialVelocity) + 180);
    }

    private void Start()
    {
        AttachToTether(initialTether);
    }
    private void OnEnable()
    {
        slowAntimatterEvent.AddListener(OnSlowAntimatter);
    }
    private void OnDisable()
    {
        slowAntimatterEvent.RemoveListener(OnSlowAntimatter);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
    protected override void OnGameStopped(object sender, EventParameters args)
    {
    }
    private void OnSlowAntimatter(object sender, EventParameters args)
    {
        isSlowed = !isSlowed;
        timeFactor = isSlowed ? antimatterTimeFactor : 1;
        animator.speed = isSlowed ? antimatterTimeFactor : 1;
        ParticleSystem.MainModule main = particleSystem.main;
        main.simulationSpeed = isSlowed ? antimatterTimeFactor : 1;
    }
}
