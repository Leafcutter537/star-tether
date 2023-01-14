using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(WormholeReachedEvent), menuName = "Events/WormholeReachedEvent")]
public class WormholeReachedEvent : BaseEvent<EventParameters>
{
}
