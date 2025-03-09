using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    public static AdsManager instance;

    [SerializeField] private string gameID = "5786608";
    [SerializeField] private string bannerAdID = "Banner_Android";
    private const string removeAdsProductID = "com.OutbreakCompany.Drinfinite.RemoveAds";
    [SerializeField] GameObject Button;
    [SerializeField] private bool TestingData;
    private void Awake()
    {
        instance = this;
        InitializeAds();


        if (PlayerPrefs.GetInt("AdsRemoved", 0) == 1)
        {
            Debug.Log("User already removed ads. Hiding ads.");
            RemoveAds();
        }
    }

    private void Update()
    {
        Button.SetActive(PlayerPrefs.GetInt("AdsRemoved", 0) != 1);
        if (TestingData) 
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs have been reset!");

        }
    }

    void InitializeAds()
    {
        Advertisement.Initialize(gameID, true, this);
    }


    public void ShowBannerAds()
    {
        if (PlayerPrefs.GetInt("AdsRemoved", 0) == 1)
        {
            Debug.Log("Ads are already removed. No need to show banners.");
            return;
        }

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load("Banner_Android", new BannerLoadOptions
        {
            loadCallback = () =>
            {
                Debug.Log("Banner ad loaded successfully!");
                Advertisement.Banner.Show("Banner_Android");

    
                GameObject adsHolder = GameObject.Find("AdsHolder");
                if (adsHolder != null)
                {
                    adsHolder.SetActive(true);
                    Debug.Log("AdsHolder activated.");
                }
            },
            errorCallback = (message) =>
            {
                Debug.LogError("Failed to load banner ad: " + message);
            }
        });

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(bannerAdID, new BannerLoadOptions
        {
            loadCallback = () =>
            {
                Debug.Log("Banner ad loaded successfully!");
                Advertisement.Banner.Show(bannerAdID);
            },
            errorCallback = (message) =>
            {
                Debug.LogError("Failed to load banner ad: " + message);
            }
        });
    }


    public void RemoveAds()
    {
        Debug.Log("Removing ads...");
        PlayerPrefs.SetInt("AdsRemoved", 1);
        PlayerPrefs.Save();
        Advertisement.Banner.Hide();
        Debug.Log("Ads should now be hidden!");
    }

    public void OnPurchaseSuccess(Product product)
    {
        if (product.definition.id == removeAdsProductID)
        {
            Debug.Log("Purchase successful: Removing ads.");
            RemoveAds();
        }
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogError($"Purchase failed for {product.definition.id}: {reason}");
    }


    public void OnTransactionsFetched()
    {
        Debug.Log("Fetching transactions...");

        if (PlayerPrefs.GetInt("AdsRemoved", 0) == 1)
        {
            Debug.Log("User has already purchased Remove Ads. Hiding ads.");
            RemoveAds();
        }
        else
        {
            Debug.Log("No previous purchase found.");
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialization Complete!");
        ShowBannerAds();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization failed: {error} - {message}");
    }


}
