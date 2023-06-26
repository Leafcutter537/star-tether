using Assets.EventSystem;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PurchaseFailedEvent), menuName = "Events/PurchaseFailedEvent")]
public class PurchaseFailedEvent : BaseEvent<EventParameters>
{
}
