using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{

    public float launchVelocity;
    public new Rigidbody2D rigidbody;
    private Tether tether;
    private Orbit orbit;
    private bool energized;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject starTetherPrefab;
    [SerializeField] private Material energizedMaterial;
    [SerializeField] private ParticleSystem.MinMaxGradient energizedGradient;
    [SerializeField] private Color energizedStartColor;
    [SerializeField] private GameObject explosion;
    [SerializeField] private ParticleSystem.MinMaxGradient blockDestructionGradient;
    [SerializeField] private Color blockDestructionColor;
    [SerializeField] private ParticleSystem.MinMaxGradient wormholeDestructionGradient;
    [SerializeField] private Color wormholeDestructionColor;
    private new ParticleSystem particleSystem;
    private GameObject starTetherInstance;

    public void Initialize(float targetAngle)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        particleSystem = GetComponentInChildren<ParticleSystem>(); 
        particleSystem.transform.rotation = Quaternion.Euler(0, 0, targetAngle + 180);

    }

    private void Awake()
    {
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
        Mesh newMesh = MeteoriteMeshCreator.Distend(meshFilter.mesh);
        meshFilter.mesh = newMesh;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (tether != null)
        {
            orbit.TimeStep(Time.deltaTime);
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

    private void OnTriggerEnter2D(Collider2D collision)
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
            }
        }
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            CreateExplosion(wormholeDestructionGradient, wormholeDestructionColor, transform.position);
            FindObjectOfType<UIController>().ShowVictory();
        }
        Block block = collision.gameObject.GetComponent<Block>();
        if (block != null)
        {
            if (energized == false)
            {
                CreateExplosion(particleSystem.colorOverLifetime.color, particleSystem.main.startColor.color, transform.position);
                Destroy(this.gameObject);
            }
            else
            {
                CreateExplosion(blockDestructionGradient, blockDestructionColor, block.transform.position);
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
}
