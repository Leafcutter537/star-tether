using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AsteroidDestroyedEvent), menuName = "Events/AsteroidDestroyedEvent")]
public class AsteroidDestroyedEvent : BaseEvent<EventParameters>
{
}
