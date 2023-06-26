using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LoadNextLevelEvent), menuName = "Events/LoadNextLevelEvent")]
public class LoadNextLevelEvent : BaseEvent<EventParameters>
{
}
