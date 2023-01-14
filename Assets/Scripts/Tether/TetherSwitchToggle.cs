using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class TetherSwitchToggle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    MeshRenderer meshRenderer;
    [Header("Colors")]
    [SerializeField] private Color onInnerColor;
    [SerializeField] private Color offInnerColor;
    [SerializeField] private Color partialInnerColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color onOuterColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color offOuterColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color partialOuterColor;
    [Header("Event References")]
    [SerializeField] private GameStoppedEvent gameStoppedEvent;
    [SerializeField] private ToggleEvent toggleEvent;
    [Header("Tether State")]
    [SerializeField] private PossibleStates possibleStates;
    private TetherState tetherState;

    enum PossibleStates
    {
        TwoState,
        ThreeState
    }
    enum TetherState
    { 
        On,
        Off,
        PartialActive,
        PartialInactive
    }

    [Header("Orbit Indicators")]
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
        ChangeState(TetherState.Off);
    }
    private void OnEnable()
    {
        gameStoppedEvent.AddListener(OnGameStopped);
    }
    private void OnDisable()
    {
        gameStoppedEvent.RemoveListener(OnGameStopped);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!StopStartController.gameIsActive)
        {
            toggleEvent.Raise(this, null);
            switch (possibleStates)
            {
                case PossibleStates.TwoState:
                    if (tetherState == TetherState.On)
                        ChangeState(TetherState.Off);
                    else
                        ChangeState(TetherState.On);
                    break;
                case PossibleStates.ThreeState:
                    switch (tetherState)
                    {
                        case TetherState.On:
                            ChangeState(TetherState.Off);
                            break;
                        case TetherState.Off:
                            ChangeState(TetherState.PartialActive);
                            break;
                        default:
                            ChangeState(TetherState.On);
                            break;
                    }
                    break;
            }
        }
    }

    private void ChangeState(TetherState newState)
    {
        tetherState = newState;
        switch (tetherState)
        {
            case (TetherState.On):
                meshRenderer.material.SetColor("_OuterColor", onOuterColor);
                meshRenderer.material.SetColor("_InnerColor", onInnerColor);
                break;
            case (TetherState.PartialActive):
                meshRenderer.material.SetColor("_OuterColor", partialOuterColor);
                meshRenderer.material.SetColor("_InnerColor", partialInnerColor);
                break;
            default:
                meshRenderer.material.SetColor("_OuterColor", offOuterColor);
                meshRenderer.material.SetColor("_InnerColor", offInnerColor);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
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

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (StarTether starTether in starTethers)
        {
            starTether.MarkForDestruction();
        }
        starTethers = new List<StarTether>();
    }
    private void OnGameStopped(object sender, EventParameters args)
    {
        if (tetherState == TetherState.PartialInactive)
            ChangeState(TetherState.PartialActive);
    }

    public bool CheckActivated()
    {
        switch (tetherState)
        {
            case TetherState.On:
                return true;
            case TetherState.Off:
                return false;
            case TetherState.PartialActive:
                ChangeState(TetherState.PartialInactive);
                return true;
            case TetherState.PartialInactive:
                return false;
        }
        return false;
    }
}
