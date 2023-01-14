using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(BarrierDestroyedEvent), menuName = "Events/BarrierDestroyedEvent")]
public class BarrierDestroyedEvent : BaseEvent<EventParameters>
{
}
