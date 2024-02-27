using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Voodoo.Tiny.Sauce.Internal.Analytics;

namespace Voodoo.Tiny.Sauce.Internal.Ads
{
    public class TSAdImageBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private TSAdData _currentAdData;
        
        private Image _img;

        
        public TSAdData CurrentAdData { set => _currentAdData = value; }


        private void Awake()
        {
            _img = GetComponent<Image>();
        }

        private void Start()
        {
            _img.sprite = _currentAdData.fakeAdContent.sprite;
        }

        public void OnPointerDown(PointerEventData eventData) { }
        public void OnPointerUp(PointerEventData eventData)
        {
            switch (_currentAdData.adType)
            {
                case TSAdType.Fake_Interstitial:
                    AnalyticsManager.TrackInterstitialClick(new AdClickEventAnalyticsInfo() {
                        AdTag = _currentAdData.adType.ToString(),
                        AdNetworkName = _currentAdData.adSdkName,
                        adPlacement = _currentAdData.adPlacement,
                        GameCount = AnalyticsStorageHelper.GetGameCount()
                    });
                    break;
                case TSAdType.Fake_RewardedVideo:
                    AnalyticsManager.TrackRewardedClick(new AdClickEventAnalyticsInfo {
                        AdTag = _currentAdData.adType.ToString(),
                        AdNetworkName = _currentAdData.adSdkName,
                        adPlacement = _currentAdData.adPlacement,
                        GameCount = AnalyticsStorageHelper.GetGameCount()
                    });
                    break;
            }
            
            Debug.Log($"Pew! Pretending to send a GameAnalytics AdEvent (AdClicked, AdType '{_currentAdData.adType.ToString()}', AdPlacement '{_currentAdData.adPlacement}') because it doesn't actually work in the editor");
            
#if UNITY_IOS
            Application.OpenURL(_currentAdData.fakeAdContent.iosUrl);
#elif UNITY_ANDROID
            Application.OpenURL(_currentAdData.fakeAdContent.androidUrl);
#endif
        }
    }
}
