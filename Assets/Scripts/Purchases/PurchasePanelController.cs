using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class PurchasePanelController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gildedLauncherTogglePanel;
    [SerializeField] private GameObject storeUnavailablePanel;
    [SerializeField] private GameObject purchaseGildedLauncherPanel;
    [SerializeField] private GameObject purchaseFailedPanel;
    [Header("Event References")]
    [SerializeField] private GildedLauncherPurchaseEvent gildedLauncherPurchaseEvent;
    [SerializeField] private StoreInitializedEvent storeInitializedEvent;
    [SerializeField] private PurchaseFailedEvent purchaseFailedEvent;
    // IAP Manager
    private MyIAPManager myIAPmanager;

    private void Awake()
    {
        myIAPmanager = FindObjectOfType<MyIAPManager>();
        if (PlayerPrefs.GetInt("GildedLauncherOwned", 0) == 1)
        {
            gildedLauncherTogglePanel.SetActive(true);
        }
        else if (myIAPmanager.storeInitailized)
        {
            purchaseGildedLauncherPanel.SetActive(true);
        }
        else
        {
            storeUnavailablePanel.SetActive(true);
        }
    }
    private void OnEnable()
    {
        gildedLauncherPurchaseEvent.AddListener(OnGildedLauncherPurchaseEvent);
        storeInitializedEvent.AddListener(OnStoreInitializedEvent);
        purchaseFailedEvent.AddListener(OnPurchaseFailedEvent);
    }
    private void OnDisable()
    {
        gildedLauncherPurchaseEvent.RemoveListener(OnGildedLauncherPurchaseEvent);
        storeInitializedEvent.RemoveListener(OnStoreInitializedEvent);
        purchaseFailedEvent.RemoveListener(OnPurchaseFailedEvent);
    }

    private void OnGildedLauncherPurchaseEvent(object sender, EventParameters args)
    {
        gildedLauncherTogglePanel.SetActive(true);
        purchaseGildedLauncherPanel.SetActive(false);
    }

    private void OnStoreInitializedEvent(object sender, EventParameters args)
    {
        if (PlayerPrefs.GetInt("GildedLauncherOwned", 0) == 0)
        {
            storeUnavailablePanel.SetActive(false);
            purchaseGildedLauncherPanel.SetActive(true);
        }
    }
   
    private void OnPurchaseFailedEvent(object sender, EventParameters args)
    {
        purchaseGildedLauncherPanel.SetActive(false);
        purchaseFailedPanel.SetActive(true);
    }

    public void BuyGildedLauncher()
    {
        myIAPmanager.BuyGildedLauncher();
    }


}
