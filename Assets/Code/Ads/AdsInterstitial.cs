using Code.Services;
using System;
using UnityEngine;

namespace Code.Ads
{
	public abstract class AdsInterstitial : MonoBehaviour, IAdsService, IInitializable
    {
        [SerializeField]
        private string m_Id;
        [SerializeField]
        private string m_Placement;

        private int m_RetryAttempt;

        public bool IsAllowedToShowAds { get; set; } = true;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public virtual void Initialize()
        {
            if (IsAllowedToShowAds)
            {
                MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
                MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
                MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
                MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
                MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
                MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
                LoadInterstitial();
            }
        }

        public bool IsReady()
        {
            return MaxSdk.IsInterstitialReady(m_Id) && IsAllowedToShowAds;
        }

        public virtual void Show()
        {
            Debug.Log($"Try Show Interstitial: {m_Placement}");
            if (IsReady())
                MaxSdk.ShowInterstitial(m_Id, m_Placement);
        }

        private void LoadInterstitial()
        {
            MaxSdk.LoadInterstitial(m_Id);
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

            // Reset retry attempt
            m_RetryAttempt = 0;
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

            m_RetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, m_RetryAttempt));

            Invoke(nameof(LoadInterstitial), (float)retryDelay);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {

        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
            LoadInterstitial();
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad.
            LoadInterstitial();
        }
    }
}