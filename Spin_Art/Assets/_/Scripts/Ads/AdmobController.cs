using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using UnityEngine;

public class AdmobController : Singleton<AdmobController>
{
    public enum RewardType { Tip, SecondChance }
    public static RewardType currentRewardType;

    private BannerView bannerViewTop;

    public InterstitialAd interstitialAd;
    public RewardedAd rewardedAd;

    [Header("Interstitial")]
    public string androidInterstitial;
    public string iosInterstitial;

    [Header("Banner")]
    public string androidBanner;
    public string iosBanner;

    [Header("Rewarded Video")]
    public string androidRewardedVideo;
    public string iosRewardedVideo;

    private bool centerBannerLoaded;

    private static bool? _isInitialized;
    public GoogleMobileAdsConsentController _consentController;

    //private const string adUnitName = "rewardedVideo";
    private const string adUnitName = "Android_Rewarded";
    private const string ActionName = "rewarded_video";

    public override void Awake()
    {
        name = nameof(AdmobController);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        RequestConfiguration requestConfiguration = new();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        InitializeGoogleMobileAds();

        return;

        // If we can request ads, we should initialize the Google Mobile Ads Unity plugin.
        if (_consentController.CanRequestAds)
        {
            InitializeGoogleMobileAds();
        }

        // Ensures that privacy and consent information is up to date.
        InitializeGoogleMobileAdsConsent();
    }

    /// <summary>
    /// Ensures that privacy and consent information is up to date.
    /// </summary>
    public void InitializeGoogleMobileAdsConsent()
    {
        Debug.Log("Google Mobile Ads gathering consent.");

        _consentController.GatherConsent((string error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed to gather consent with error: " +
                    error);
            }
            else
            {
                Debug.Log("Google Mobile Ads consent updated: "
                    + ConsentInformation.ConsentStatus);
            }

            if (_consentController.CanRequestAds)
            {
                InitializeGoogleMobileAds();
            }
        });
    }

    private void InitializeGoogleMobileAds()
    {
        // The Google Mobile Ads Unity plugin needs to be run only once and before loading any ads.
        if (_isInitialized.HasValue)
        {
            return;
        }

        _isInitialized = false;

        // Initialize the Google Mobile Ads Unity plugin.
        Debug.Log("Google Mobile Ads Initializing.");
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                _isInitialized = null;
                return;
            }

            // If you use mediation, you can check the status of each adapter.
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }

            Debug.Log("Google Mobile Ads initialization complete.");
            _isInitialized = true;
            StartCoroutine(DelayLoadBanner());
            RequestInterstitial();
            //RequestRewardBasedVideo();
        });
    }

    private IEnumerator DelayLoadBanner()
    {
        yield return new WaitForSeconds(0.1f);
        RequestBanner();
    }

    public void RequestBanner()
    {
        bannerViewTop?.Destroy();

        // Adaptive Banner
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerViewTop = new BannerView(androidBanner.Trim(), adSize, AdPosition.Bottom);

        bannerViewTop.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded.");
        };
        bannerViewTop.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("Banner ad failed to load with error: " + error.GetMessage());
        };
        bannerViewTop.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner ad recorded an impression.");
        };
        bannerViewTop.OnAdClicked += () =>
        {
            Debug.Log("Banner ad recorded a click.");
        };
        bannerViewTop.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner ad opening.");
        };
        bannerViewTop.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner ad closed.");
        };
        bannerViewTop.OnAdPaid += (AdValue adValue) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}", "Banner ad received a paid event.", adValue.CurrencyCode, adValue.Value); Debug.Log(msg);
        };

        bannerViewTop.LoadAd(new AdRequest());
    }

    public void RequestInterstitial()
    {
        Debug.Log("Can show Interstitial ad " + interstitialAd?.CanShowAd());
        if (interstitialAd != null)
        {
            if (!interstitialAd.CanShowAd())
            {
                Debug.Log("Interstitial Ad Unit contains Ad that has been shown to user, so discard it");
                interstitialAd.Destroy();
                interstitialAd = null;
            }
            else
            {
                Debug.Log("Interstitial Ad Unit contains Ad that has not been shown to user, so dont put in new Ad Request");
                return;
            }
        }
        else
        {
            Debug.Log("Empty Interstitial Ad Unit");
        }

        InterstitialAd.Load(androidInterstitial, new AdRequest(), (InterstitialAd ad, LoadAdError loadAdError) =>
        {
            if (loadAdError != null)
            {
                Debug.Log("Interstitial ad failed to load with error: " + loadAdError.GetMessage());
                return;
            }
            else if (ad == null)
            {
                Debug.Log("Interstitial ad failed to load.");
                return;
            }

            Debug.Log("Interstitial load success");
            interstitialAd = ad;

            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Interstitial ad opening.");
            };
            ad.OnAdFullScreenContentClosed += () =>
            {
                RequestInterstitial();
                Debug.Log("Interstitial ad closed.");
            };
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Interstitial ad recorded an impression.");
            };
            ad.OnAdClicked += () =>
            {
                Debug.Log("Interstitial ad recorded a click.");
            };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.Log("Interstitial ad failed to show with error: " + error.GetMessage());
            };
            ad.OnAdPaid += (AdValue adValue) =>
            {
                string msg = string.Format("{0} (currency: {1}, value: {2}", "Interstitial ad received a paid event.", adValue.CurrencyCode, adValue.Value);
                Debug.Log(msg);
            };
        });
    }
    public void RequestRewardBasedVideo()
    {
        Debug.Log("Can show Rewarded ad " + rewardedAd?.CanShowAd());

        if (rewardedAd != null)
        {
            if (!rewardedAd.CanShowAd())
            {
                Debug.Log("Rewarded Ad Unit contains Ad that has been shown to user, so discard it");
                rewardedAd.Destroy();
                rewardedAd = null;
            }
            else
            {
                Debug.Log("Rewarded Ad Unit contains Ad that has not been shown to user, so dont put in new Ad Request");
                return;
            }
        }
        else
        {
            Debug.Log("Empty Rewarded Ad Unit");
        }

        RewardedAd.Load(androidRewardedVideo, new AdRequest(), (RewardedAd ad, LoadAdError loadError) =>
        {
            if (loadError != null)
            {
                Debug.Log("Rewarded ad failed to load with error: " + loadError.GetMessage());
                return;
            }
            else if (ad == null)
            {
                Debug.Log("Rewarded ad failed to load.");
                return;
            }

            Debug.Log("Rewarded load success");
            rewardedAd = ad;

            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad opening.");
            };
            ad.OnAdFullScreenContentClosed += () =>
            {
                RequestRewardBasedVideo();
                Debug.Log("Rewarded ad closed.");
            };
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad recorded a click.");
            };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.Log("Rewarded ad failed to show with error: " + error.GetMessage());
            };
            ad.OnAdPaid += (AdValue adValue) =>
            {
                string msg = string.Format("{0} (currency: {1}, value: {2}", "Rewarded ad received a paid event.", adValue.CurrencyCode, adValue.Value);
                Debug.Log(msg);
            };
        });
    }

    public void ShowInterstitial(InterstitialAd ad)
    {
        if (ad != null && ad.CanShowAd())
            ad.Show();
    }

    public void ShowBanner() => bannerViewTop?.Show();

    public void HideBanner() => bannerViewTop?.Hide();

    public bool ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            return true;
        }
        //Advertisement.Show();
        return false;
    }

    public void ShowRewardBasedVideo()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
            rewardedAd.Show((Reward reward) =>
            {
                RewardUserFinal();
            });
        else
            print("Reward based video ad is not ready yet");
    }

    private void RewardUserFinal()
    {
        switch (currentRewardType)
        {
            case RewardType.Tip:
                break;
            case RewardType.SecondChance:
                break;
        }
    }
}
