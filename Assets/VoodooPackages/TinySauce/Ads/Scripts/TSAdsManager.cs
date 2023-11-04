using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voodoo.Tiny.Sauce.Internal.Ads
{
    public class TSAdsManager : MonoBehaviour
    {
        private static bool _areAdsEnabled = true;
        
        private static TSAdDisplayer adDisplayerPrefab;
        
        private static TSAdDisplayer adDisplayer;
        private static TSFSDisplayConditions fsDisplayConditions;
        
        private static TSAdData _currentAdData;
        
        private const string AD_SDK_NAME = "vdfats";
        
        public static bool AreAdsEnabled { get => _areAdsEnabled; }


        private void Awake()
        {
            InitAdDisplayer();
            InitFSDisplayConditions();
            _currentAdData = new TSAdData();
        }

        
#region [PUBLIC_FUNCTIONS]
        /// <summary>
        /// Enables or disables TinySauce Ads.
        /// </summary>
        /// <param name="areAdsEnabled"></param>
        public static void ToggleAds(bool areAdsEnabled)
        {
            _areAdsEnabled = areAdsEnabled;
            //Debug.Log($" ===== AreAdsEnabled = {_areAdsEnabled} ===== ");
        }

        /// <summary>
        /// Set custom conditions to display Interstitial ad.
        /// </summary>
        /// <param name="delayBeforeFirstInterstitial">Delay before a first Interstitial can be displayed - in seconds</param>
        /// <param name="delayBetweenInterstitials">Delay between 2 Interstitials (also depends on maxLevelsBetweenInterstitials) - in seconds</param>
        /// <param name="maxLevelsBetweenInterstitials">Number of levels played before an Interstitial can be displayed (also depends on delayBetweenInterstitials)</param>
        /// <param name="delayBetweenRewardedVideoAndInterstitial">Delay after displaying a Rewarded Video before an Interstitial can be displayed - in seconds</param>
        public static void SetFSDisplayConditions(
            int delayBeforeFirstInterstitial,
            int delayBetweenInterstitials,
            int maxLevelsBetweenInterstitials,
            int delayBetweenRewardedVideoAndInterstitial = -1)
        {
            fsDisplayConditions = new TSFSDisplayConditions(
                delayBeforeFirstInterstitial,
                delayBetweenInterstitials, 
                maxLevelsBetweenInterstitials,
                delayBetweenRewardedVideoAndInterstitial);
        }

        /// <summary>
        /// Display an Interstitial ad if the conditions are met.
        /// </summary>
        /// <param name="onAdClosed">Callback when an ad is closed</param>
        /// <param name="adPlacement">Location and/or purpose of the ad - MAX 64 CHAR - e.g. "between-levels" or "end-level-bonus-coins"</param>
        public static void ShowInterstitial(Action onAdClosed = null, string adPlacement = "not-specified")
        {
            if (_areAdsEnabled && AreFSDisplayConditionsMet())
            {
                OpenAdDisplayer(onAdClosed, TSAdType.Fake_Interstitial, adPlacement);
                fsDisplayConditions.FSDisplayed();
            }
        }
        
        /// <summary>
        /// Display a Rewarded Video ad if the conditions are met.
        /// </summary>
        /// <param name="onAdClosed">Callback when an ad is closed (can be used to give a reward after a Rewarded Video)</param>
        /// <param name="adPlacement">Location and/or purpose of the ad - MAX 64 CHAR - e.g. "between-levels" or "end-level-bonus-coins"</param>
        public static void ShowRewardedVideo(Action onAdClosed = null, string adPlacement = "not-specified")
        {
            if (_areAdsEnabled)
            {
                OpenAdDisplayer(onAdClosed, TSAdType.Fake_RewardedVideo, adPlacement);
                fsDisplayConditions.RVDisplayed();
            }
        }
#endregion


#region [ADS]

        private void InitAdDisplayer()
        {
            if (adDisplayerPrefab == null)
            {
                TSAdDisplayer[] adDisplayerList = Resources.LoadAll<TSAdDisplayer>("Prefabs");
                adDisplayerPrefab = adDisplayerList[0];
            }
            
            if (adDisplayerPrefab == null)
                Debug.LogError("There is no AdDisplayer prefab in the 'Assets/VoodooPackages/TinySauce/Ads/Resources/Prefabs' folder");
        }
        
        private void InitFSDisplayConditions()
        {
            fsDisplayConditions = new TSFSDisplayConditions();
        }

        private static void OpenAdDisplayer(Action onAdClosed, TSAdType adType, string adPlacement)
        {
            if (TSAdDisplayer.Instance == null)
            {
                InitAdInfo(adType, adPlacement);
                adDisplayer = Instantiate(adDisplayerPrefab);
                if (onAdClosed != null) adDisplayer.On_AdClosed += onAdClosed;
            }
        }
        
        private static bool AreFSDisplayConditionsMet()
        {
            if (fsDisplayConditions != null)
                return fsDisplayConditions.AreConditionsMet();
            else
            {
                Debug.LogError("No fsDisplayConditions has been created (= null)");
                return false;
            }
        }

        private static void InitAdInfo(TSAdType adType, string adPlacement)
        {
            _currentAdData.adType = adType;
            _currentAdData.adSdkName = AD_SDK_NAME;
            _currentAdData.adPlacement = adPlacement;
            TSAdDisplayer.CurrentAdData = _currentAdData;
        }
#endregion
    }
}