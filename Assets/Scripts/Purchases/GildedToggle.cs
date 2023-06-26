
using UnityEngine;

public class GildedToggle : PurchaseToggle
{
    [SerializeField] private GildedLauncherOnEvent gildedLauncherOnEvent;
    [SerializeField] private GildedLauncherOffEvent gildedLauncherOffEvent;
    protected override void OnToggleOff()
    {
        PlayerPrefs.SetInt("GildedLauncherOn", 0);
        gildedLauncherOffEvent.Raise(this, null);
    }

    protected override void OnToggleOn()
    {
        PlayerPrefs.SetInt("GildedLauncherOn", 1);
        gildedLauncherOnEvent.Raise(this, null);
    }
}
