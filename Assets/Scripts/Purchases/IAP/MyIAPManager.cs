using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

public class MyIAPManager : MonoBehaviour, IStoreListener
{

    private IStoreController controller;
    private IExtensionProvider extensions;
    public string environment;
    public string gildedLauncherID;
    [SerializeField] private GildedLauncherPurchaseEvent gildedLauncherPurchaseEvent;
    [SerializeField] private PurchaseFailedEvent purchaseFailedEvent;
    [SerializeField] private StoreInitializedEvent storeInitializedEvent;
    public bool storeInitailized;

    private async void Awake()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch 
        {
            
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(gildedLauncherID, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeInitailized = true;
        storeInitializedEvent.Raise(this, null);
        this.controller = controller;
        this.extensions = extensions;
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Product product = e.purchasedProduct;

        if (product.definition.id == gildedLauncherID)
        {
            PlayerPrefs.SetInt("GildedLauncherOwned", 1);
            gildedLauncherPurchaseEvent.Raise(this, null);
        }

        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        purchaseFailedEvent.Raise(this, null);
    }

    public void BuyGildedLauncher()
    {
        controller.InitiatePurchase(gildedLauncherID);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    }
}