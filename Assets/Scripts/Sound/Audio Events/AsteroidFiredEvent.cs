using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AsteroidFiredEvent), menuName = "Events/AsteroidFiredEvent")]
public class AsteroidFiredEvent : BaseEvent<EventParameters>
{
}
