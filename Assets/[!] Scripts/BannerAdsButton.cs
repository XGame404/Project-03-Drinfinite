using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class BannerAdsButton : MonoBehaviour
{
    [SerializeField] private GameObject removeAdsButton;
    public TMP_Text priceText;
    private void Start()
    {
        if (CodelessIAPStoreListener.Instance != null && CodelessIAPStoreListener.Instance.StoreController != null)
        {
            var product = CodelessIAPStoreListener.Instance.StoreController.products.WithID("com.OutbreakCompany.Drinfinite.removeads");

            if (product != null)
            {
                priceText.text = product.metadata.localizedPriceString;
            }
            else
            {
                priceText.text = "N/A";
            }
            if (removeAdsButton != null)
                removeAdsButton.SetActive(PlayerPrefs.GetInt("AdsRemoved", 0) == 0);
        }
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == "com.OutbreakCompany.Drinfinite.removeads")
        {
            StartCoroutine(ProcessRemoveAdsCoroutine());
        }
    }

    private IEnumerator ProcessRemoveAdsCoroutine()
    {
        yield return new WaitForEndOfFrame();
        BannerAdsManager.instance.RemoveAds();
        if (removeAdsButton != null)
            removeAdsButton.SetActive(false);
        Debug.Log("Ads removed!");
    }
}
