using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameStoppedEvent), menuName = "Events/GameStoppedEvent")]
public class GameStoppedEvent : BaseEvent<EventParameters>
{
}
