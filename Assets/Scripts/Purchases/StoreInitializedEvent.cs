using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(StoreInitializedEvent), menuName = "Events/StoreInitializedEvent")]
public class StoreInitializedEvent : BaseEvent<EventParameters>
{
}
