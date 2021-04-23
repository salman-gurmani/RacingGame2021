using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class InAppManager : MonoBehaviour, IStoreListener
{
    public static InAppManager instance;
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static string Pack_1Kcash = Constants.coins_1;
    public static string Pack_3Kcash = Constants.coins_2;
    public static string Pack_5Kcash = Constants.coins_3;
    public static string Pack_20Kcash = Constants.coins_4;
    public static string Pack_50Kcash = Constants.coins_5;
    public static string Unlock_AllCars = Constants.unlockPlayerObj;
    public static string Unlock_AllLevels = Constants.unlockLevels;
    public static string BUY_FuelTank = Constants.buyFuel;

    public static string kProductIDNonConsumable = "nonconsumable";
    public static string kProductIDSubscription = "subscription";

    // Apple App Store-specific product identifier for the subscription product.
    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        builder.AddProduct(Pack_1Kcash, ProductType.Consumable);
        builder.AddProduct(Pack_3Kcash, ProductType.Consumable);
        builder.AddProduct(Pack_5Kcash, ProductType.Consumable);
        builder.AddProduct(Pack_20Kcash, ProductType.Consumable);
        builder.AddProduct(Pack_50Kcash, ProductType.Consumable);
        builder.AddProduct(BUY_FuelTank, ProductType.Consumable);

        // Continue adding the non-consumable product.

        builder.AddProduct(Unlock_AllCars, ProductType.NonConsumable);
        builder.AddProduct(Unlock_AllLevels, ProductType.NonConsumable);
        builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
            { kProductNameAppleSubscription, AppleAppStore.Name },
            { kProductNameGooglePlaySubscription, GooglePlay.Name },
        });

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    // Buy the consumable product using its general identifier. Expect a response either 
    // through ProcessPurchase or OnPurchaseFailed asynchronously.
    public void BuyCash_1000()
    {
        BuyProductID(Pack_1Kcash);
    }
    public void BuyCash_3000()
    {
        BuyProductID(Pack_3Kcash);
    }
    public void BuyCash_5000()
    {
        BuyProductID(Pack_5Kcash);
    }
    public void BuyCash_20000()
    {
        BuyProductID(Pack_20Kcash);
    }
    public void BuyCash_50000()
    {
        BuyProductID(Pack_50Kcash);
    }
    public void Buy_UnlockAllCars()
    {
        BuyProductID(Unlock_AllCars);
    }
    public void Buy_UnlockAllLevels()
    {
        BuyProductID(Unlock_AllLevels);
    }
    public void Buy_FuelTank()
    {
        BuyProductID(BUY_FuelTank);
    }

    public void BuyNonConsumable()
    {
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(kProductIDNonConsumable);
    }


    public void BuySubscription()
    {
        // Buy the subscription product using its the general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        // Notice how we use the general product identifier in spite of this ID being mapped to
        // custom store-specific identifiers above.
        BuyProductID(kProductIDSubscription);
    }


    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
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
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, Pack_1Kcash, StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.GoldCoins += 1000;
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Pack_3Kcash, StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.GoldCoins += 3000;
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        else if (String.Equals(args.purchasedProduct.definition.id, Pack_5Kcash, StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.GoldCoins += 5000;
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        else if (String.Equals(args.purchasedProduct.definition.id, Pack_20Kcash, StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.GoldCoins += 20000;
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        else if (String.Equals(args.purchasedProduct.definition.id, Pack_50Kcash, StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.GoldCoins += 50000;
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        else if (String.Equals(args.purchasedProduct.definition.id, Unlock_AllCars, StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.UnlockAllPlayerObj();
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        else if (String.Equals(args.purchasedProduct.definition.id, Unlock_AllLevels, StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.UnlockAllLevels();
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        else if (String.Equals(args.purchasedProduct.definition.id, BUY_FuelTank, StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.FuelTank = 8;
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}