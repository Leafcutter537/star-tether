using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [Header("Asteroid Properties")]
    public new Rigidbody2D rigidbody;
    private Tether tether;
    private Orbit orbit;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject starTetherPrefab;
    [Header("Energized")]
    [SerializeField] private Material energizedMaterial;
    private bool energized;
    [SerializeField] private ParticleSystem.MinMaxGradient energizedGradient;
    [SerializeField] private Color energizedStartColor;
    [Header("Block Destruction")]
    [SerializeField] private GameObject explosion;
    [SerializeField] private ParticleSystem.MinMaxGradient blockDestructionGradient;
    [SerializeField] private Color blockDestructionColor;
    [Header("Wormhole Destruction")]
    [SerializeField] private ParticleSystem.MinMaxGradient wormholeDestructionGradient;
    [SerializeField] private Color wormholeDestructionColor;
    [Header("Particle System")]
    [SerializeField] protected new ParticleSystem particleSystem;
    private GameObject starTetherInstance;
    [Header("Event References")]
    [SerializeField] private AsteroidDestroyedEvent asteroidDestroyedEvent;
    [SerializeField] private GameStoppedEvent gameStoppedEvent;
    [SerializeField] private EnergyPickupEvent energyPickupEvent;
    [SerializeField] private WormholeReachedEvent wormholeReachedEvent;
    [SerializeField] private TetherEvent tetherEvent;
    [SerializeField] private BarrierDestroyedEvent barrierDestroyedEvent;
    [Header("Time Dilation")]
    protected float timeFactor = 1;

    public void Initialize(float targetAngle)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        particleSystem = GetComponentInChildren<ParticleSystem>(); 
        particleSystem.transform.rotation = Quaternion.Euler(0, 0, targetAngle + 180);

    }

    protected virtual void Awake()
    {
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
        Mesh newMesh = MeteoriteMeshCreator.Distend(meshFilter.mesh);
        meshFilter.mesh = newMesh;
    }
    private void OnEnable()
    {
        gameStoppedEvent.AddListener(OnGameStopped);
    }
    private void OnDisable()
    {
        gameStoppedEvent.RemoveListener(OnGameStopped);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (tether != null)
        {
            orbit.TimeStep(Time.fixedDeltaTime * timeFactor);
            Vector2 originalPosition = new Vector2(rigidbody.position.x, rigidbody.position.y);
            Vector2 newPosition = tether.rigidbody.position + orbit.GetPositionRelativeToTether();
            rigidbody.MovePosition(newPosition);
            Vector2 movementDirectionVector = (originalPosition - newPosition).normalized;
            particleSystem.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, movementDirectionVector));
            SetStarTetherPositionAndAngle();
        }
    }


    public void AttachToTether(TetherSwitch tetherSwitch)
    {
        tether = tetherSwitch.tether;
        Vector2 tetherToObject = rigidbody.position - tether.rigidbody.position;
        float angle = Vector2.SignedAngle(rigidbody.velocity, Vector2.Perpendicular(tetherToObject));
        angle = angle * Mathf.PI / 180;
        float perpendicularVelocity = rigidbody.velocity.magnitude * Mathf.Cos(angle);
        float phi = Mathf.Acos(tetherToObject.normalized.x);
        if (tetherToObject.y < 0)
        {
            phi = -1 * phi;
        }
        orbit = new Orbit(perpendicularVelocity, phi, tetherSwitch.radius);
        rigidbody.velocity = Vector2.zero;
        starTetherInstance = Instantiate(starTetherPrefab, Vector3.zero, Quaternion.identity);
        ParticleSystem particleSystem = starTetherInstance.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        float scale = (tetherSwitch.radius - 3f) * 0.4f + 1;
        shapeModule.scale = new Vector3(1, scale, 1);
        emissionModule.rateOverTime = 60 * scale;
        SetStarTetherPositionAndAngle();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        TetherSwitch tetherSwitch = collision.gameObject.GetComponent<TetherSwitch>();
        if (tetherSwitch != null)
        {
            if (tetherSwitch.IsActivated())
            {
                if (tether == null)
                    AttachToTether(tetherSwitch);
                else
                    UnattachFromTether(tetherSwitch);
                tetherSwitch.TriggerSwitchAnimation();
                tetherEvent.Raise(this, null);
            }
        }
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            CreateExplosion(wormholeDestructionGradient, wormholeDestructionColor, transform.position);
            wormholeReachedEvent.Raise(this, null);
            FindObjectOfType<UIController>().ShowVictory();
        }
        Block block = collision.gameObject.GetComponent<Block>();
        if (block != null)
        {
            if (energized == false)
            {
                CreateExplosion(particleSystem.colorOverLifetime.color, particleSystem.main.startColor.color, transform.position);
                asteroidDestroyedEvent.Raise(this, null);
                Destroy(this.gameObject);
            }
            else
            {
                CreateExplosion(blockDestructionGradient, blockDestructionColor, block.transform.position);
                barrierDestroyedEvent.Raise(this, null);
                Destroy(block.gameObject);
            }
        }
        Energy energy = collision.gameObject.GetComponent<Energy>();
        if (energy != null)
        {
            CreateExplosion(energizedGradient, energizedStartColor, energy.transform.position);
            meshRenderer.material = energizedMaterial;
            ParticleSystem.ColorOverLifetimeModule colorOverLife = particleSystem.colorOverLifetime;
            colorOverLife.color = energizedGradient;
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = energizedStartColor;
            Destroy(energy.gameObject);
            energized = true;
            energyPickupEvent.Raise(this, null);
        }
        if (collision.gameObject.tag == "Antimatter")
        {
            CreateExplosion(particleSystem.colorOverLifetime.color, particleSystem.main.startColor.color, transform.position);
            asteroidDestroyedEvent.Raise(this, null);
            Destroy(this.gameObject);
        }
    }

    public void UnattachFromTether(TetherSwitch tetherSwitch)
    {
        if (tether == tetherSwitch.tether)
        {
            rigidbody.velocity = orbit.GetReleaseVelocity();
            rigidbody.velocity = tetherSwitch.HomeOnTarget(rigidbody);
            tether = null;
            orbit = null;
            RemoveStarTether();
        }
        
    }

    public void RemoveStarTether()
    {
        if (starTetherInstance != null)
        {
            starTetherInstance.GetComponent<StarTether>().MarkForDestruction();
        }
    }

    private void SetStarTetherPositionAndAngle()
    {
        Vector2 differenceVector = new Vector2(tether.transform.position.x - transform.position.x,
            tether.transform.position.y - transform.position.y);
        differenceVector = differenceVector.normalized;

        Vector3 averageVector = (tether.transform.position + transform.position) / 2;

        starTetherInstance.transform.position = new Vector3(averageVector.x - 0.3f * differenceVector.x,
            averageVector.y - 0.3f * differenceVector.y,
            averageVector.z);

        float angle = Vector2.SignedAngle(Vector2.right, differenceVector);

        starTetherInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    private void CreateExplosion(ParticleSystem.MinMaxGradient gradient, Color color, Vector3 position)
    {
        GameObject explosionObject = Instantiate(explosion, position, Quaternion.identity);
        ParticleSystem explosionParticleSystem = explosionObject.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = explosionParticleSystem.main;
        ParticleSystem.ColorOverLifetimeModule colorOverLife = explosionParticleSystem.colorOverLifetime;
        StarTether starTether = explosionObject.GetComponent<StarTether>();
        main.startColor = color;
        colorOverLife.color = gradient;
        starTether.MarkForDestruction();
        explosionParticleSystem.Play();
    }

    protected virtual void OnGameStopped(object sender, EventParameters args)
    {
        RemoveStarTether();
        Destroy(gameObject);
    }
}
