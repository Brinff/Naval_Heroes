using System;
using UnityEngine;
using Voodoo.Tiny.Sauce.Internal.Analytics;

namespace Voodoo.Tiny.Sauce.Internal.Ads
{
    public class TSFSDisplayConditions
    {
        private const string PREFS_FIRST_APP_LAUNCH = "TinySauce.Interstitial.FirstAppLaunch";
        
        private const int DEFAULT_DELAY_BEFORE_FIRST_FS = 30;
        private const int DEFAULT_DELAY_BETWEEN_FS = 30;
        private const int DEFAULT_MAX_LEVELS_BETWEEN_FS = 3;
        private const int DEFAULT_DELAY_BETWEEN_RV_AND_FS = 5;

        
        private bool _hasFirstAdBeenDisplayed;
        private int _delayBeforeFirstFS;
        
        private float _lastFSTime;
        private int _delayBetweenFS;
        
        private int _levelsPlayedOnLastFS;
        private int _maxLevelsPlayedBetweenFS;
        
        private float _lastRVTime;
        private int _delayBetweenRVAndFS;

        
        public TSFSDisplayConditions() : this(
            DEFAULT_DELAY_BEFORE_FIRST_FS,
            DEFAULT_DELAY_BETWEEN_FS,
            DEFAULT_MAX_LEVELS_BETWEEN_FS,
            DEFAULT_DELAY_BETWEEN_RV_AND_FS)
        { }

        public TSFSDisplayConditions(
            int delayBeforeFirstFS,
            int delayBetweenFS,
            int maxLevelsBetweenFS,
            int delayBetweenRVAndFS)
        {
            _hasFirstAdBeenDisplayed = PlayerPrefs.HasKey(PREFS_FIRST_APP_LAUNCH);            
            _delayBeforeFirstFS = delayBeforeFirstFS;

            _lastFSTime = Time.unscaledTime;
            _delayBetweenFS = delayBetweenFS;

            _levelsPlayedOnLastFS = AnalyticsStorageHelper.GetGameCount();
            _maxLevelsPlayedBetweenFS = maxLevelsBetweenFS;

            _lastRVTime = Time.unscaledTime - delayBetweenRVAndFS;
            _delayBetweenRVAndFS = delayBetweenRVAndFS;
            if (_delayBetweenRVAndFS == -1) _delayBetweenRVAndFS = DEFAULT_DELAY_BETWEEN_RV_AND_FS;
///**/            Debug.Log($" ===== FSDisplayConditions INITIALIZED :: {_delayBeforeFirstFS}, {_delayBetweenFS}, {_maxLevelsPlayedBetweenFS}, {_delayBetweenRVAndFS} ===== ");
        }

        public bool AreConditionsMet()
        {
            bool isFSAvailableAfterRV = Time.unscaledTime >= _lastRVTime + _delayBetweenRVAndFS;
            bool isFSAvailableAfterFS = Time.unscaledTime >= _lastFSTime + _delayBetweenFS;
            bool hasEnoughLevelPlayed = AnalyticsStorageHelper.GetGameCount() - _levelsPlayedOnLastFS >= _maxLevelsPlayedBetweenFS;

            if (!isFSAvailableAfterRV)
            {
                Debug.Log($"! Ad not displayed ! -> delay after Rewarded Video not reached yet: {Time.unscaledTime - _lastRVTime}/{_delayBetweenRVAndFS}");
                return false;
            }
            if (!_hasFirstAdBeenDisplayed)
            {
                Debug.Log($"! Ad not displayed ! -> delay before first interstitial ad not reached yet: {Time.unscaledTime}/{_delayBeforeFirstFS}");
                return Time.unscaledTime >= _delayBeforeFirstFS;
            }
            if (!isFSAvailableAfterFS && !hasEnoughLevelPlayed)
            {
                Debug.Log($"! Ad not displayed ! -> delay or number of level between interstitial ads not reached yet: DelayBetweenInterstitialAds = {Time.unscaledTime - _lastFSTime}/{_delayBetweenFS} || LevelsBetweenInterstitials = {AnalyticsStorageHelper.GetGameCount() - _levelsPlayedOnLastFS}/{_maxLevelsPlayedBetweenFS}");
            }
            return isFSAvailableAfterFS || hasEnoughLevelPlayed;
        }
        
        public void FSDisplayed()
        {
            SetFirstAdDisplayedIfNotAlready();

            _levelsPlayedOnLastFS = AnalyticsStorageHelper.GetGameCount();
            _lastFSTime = Time.unscaledTime;
        }
        
        public void RVDisplayed()
        {
            SetFirstAdDisplayedIfNotAlready();
            
            _lastRVTime = Time.unscaledTime;
        }

        private void SetFirstAdDisplayedIfNotAlready()
        {
            if (!_hasFirstAdBeenDisplayed)
            {
                _hasFirstAdBeenDisplayed = true;
                PlayerPrefs.SetInt(PREFS_FIRST_APP_LAUNCH, 1);
            }
        }
    }
}