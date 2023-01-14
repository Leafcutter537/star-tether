using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EnergyPickupEvent), menuName = "Events/EnergyPickupEvent")]
public class EnergyPickupEvent : BaseEvent<EventParameters>
{
}
