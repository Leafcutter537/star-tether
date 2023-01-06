using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TetherSwitchToggle : MonoBehaviour, IPointerClickHandler
{

    public bool isActivated;
    MeshRenderer meshRenderer;
    [SerializeField] private Color onInnerColor;
    [SerializeField] private Color offInnerColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color onOuterColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color offOuterColor;

    [SerializeField] private GameObject orbitIndicatorPrefab;
    [SerializeField] private GameObject perpendicularIndicatorPrefab;
    [SerializeField] private float perpendicularIndicatorSize;
    [SerializeField] private int perpendicularIndicatorCount;

    private List<StarTether> starTethers;

    private Tether tether;
    private TetherSwitch tetherSwitch;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("_OuterColor", offOuterColor);
        meshRenderer.material.SetColor("_InnerColor", offInnerColor);
        tether = GetComponentInParent<Tether>();
        tetherSwitch = GetComponentInParent<TetherSwitch>();
        starTethers = new List<StarTether>();
    }


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!StopStartController.gameIsActive)
        {
            if (isActivated)
            {
                isActivated = false;
                meshRenderer.material.SetColor("_OuterColor", offOuterColor);
                meshRenderer.material.SetColor("_InnerColor", offInnerColor);
            }
            else
            {
                isActivated = true;
                meshRenderer.material.SetColor("_OuterColor", onOuterColor);
                meshRenderer.material.SetColor("_InnerColor", onInnerColor);
            }
        }
    }

    void OnMouseOver()
    {
        if (starTethers.Count == 0)
        {
            StarTether orbitalStarTether = Instantiate(orbitIndicatorPrefab, tether.transform.position, Quaternion.identity).GetComponent<StarTether>();
            ParticleSystem particleSystem = orbitalStarTether.GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            shapeModule.radius = tetherSwitch.radius;
            emissionModule.rateOverTime = 200 * shapeModule.radius / 3;
            starTethers.Add(orbitalStarTether);


            Vector2 tetherToSwitch = new Vector2(tether.transform.position.x - transform.position.x,
                tether.transform.position.y - transform.position.y);
            tetherToSwitch = tetherToSwitch.normalized;
            float angle = Vector2.SignedAngle(Vector2.up, tetherToSwitch);
            StarTether perpStarTether = Instantiate(perpendicularIndicatorPrefab, transform.position, Quaternion.identity).GetComponent<StarTether>();
            perpStarTether.transform.rotation = Quaternion.Euler(0, 0, angle);
            starTethers.Add(perpStarTether);

            for (int i = 1; i < perpendicularIndicatorCount; i++)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector2 displacement = Vector2.Perpendicular(tetherToSwitch) * i * j * perpendicularIndicatorSize;
                    Vector2 newPosition = new Vector3(transform.position.x + displacement.x,
                        transform.position.y + displacement.y,
                        0);
                    StarTether starTether = Instantiate(perpendicularIndicatorPrefab, newPosition, Quaternion.identity).GetComponent<StarTether>();
                    starTether.transform.rotation = Quaternion.Euler(0, 0, angle);
                    starTethers.Add(starTether);
                }
            }


        }


    }

    void OnMouseExit()
    {
        foreach (StarTether starTether in starTethers)
        {
            starTether.MarkForDestruction();
        }
        starTethers = new List<StarTether>();
    }
}
