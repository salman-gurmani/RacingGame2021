using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppHandler : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.


    void Start()
    {
        if (m_StoreController == null)
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //builder.AddProduct(gold_1000, ProductType.Consumable);
        builder.AddProduct(Constants.coins_1, ProductType.Consumable);
        builder.AddProduct(Constants.coins_2, ProductType.Consumable);
        builder.AddProduct(Constants.unlockPlayerObj, ProductType.NonConsumable);
        builder.AddProduct(Constants.unlockLevels, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            UnityEngine.Purchasing.Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Toolbox.GameManager.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                Toolbox.GameManager.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            Toolbox.GameManager.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Toolbox.GameManager.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Toolbox.GameManager.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Toolbox.GameManager.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Toolbox.GameManager.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    #region CallBack Methods

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        Toolbox.GameManager.Log("INAPP Initialization Failed!");

    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs args)
    {
        //if (String.Equals(args.purchasedProduct.definition.id, gold_1000, StringComparison.Ordinal))
        //{
        //    Toolbox.GameManager.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    //Toolbox.GameManager.AnalyticEvent_Business("Dollar", 1, "Consumeable", "Gold_1000", "None");
        //}

        if (String.Equals(args.purchasedProduct.definition.id, Constants.coins_1, StringComparison.Ordinal))
        {
            Toolbox.GameManager.AnalyticEvent_Business("Non-Consumeable", 200, "no_ads");
            Toolbox.GameManager.Log("Purchase Success = NoAds");

            Toolbox.DB.prefs.GoldCoins += 1000;
        }

        else if (String.Equals(args.purchasedProduct.definition.id, Constants.coins_2, StringComparison.Ordinal))
        {
            Toolbox.GameManager.AnalyticEvent_Business("Non-Consumeable", 550, "unlockall_n_noads");
            Toolbox.GameManager.Log("Purchase Success = NoAds");

            Toolbox.DB.prefs.GoldCoins += 5000;
        }

        else if (String.Equals(args.purchasedProduct.definition.id, Constants.unlockPlayerObj, StringComparison.Ordinal))
        {
            Toolbox.GameManager.AnalyticEvent_Business("Non-Consumeable", 550, "unlockall_n_noads");
            Toolbox.GameManager.Log("Purchase Success = NoAds");

            Toolbox.DB.prefs.UnlockAllPlayerObj();
        }

        else if (String.Equals(args.purchasedProduct.definition.id, Constants.unlockLevels, StringComparison.Ordinal))
        {
            Toolbox.GameManager.AnalyticEvent_Business("Non-Consumeable", 550, "unlockall_n_noads");
            Toolbox.GameManager.Log("Purchase Success = NoAds");

            Toolbox.DB.prefs.UnlockAllLevels();
        }

        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Toolbox.GameManager.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 

        Toolbox.DB.Save_Binary_Prefs();

        return PurchaseProcessingResult.Complete;
    }

    void IStoreListener.OnPurchaseFailed(UnityEngine.Purchasing.Product i, PurchaseFailureReason p)
    {
        Toolbox.GameManager.Log("INAPP Purchase Failed! Reason = " + p + " _of Product = " + i);
    }

    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Toolbox.GameManager.Log("INAPP Initialized!");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }

    #endregion

}
