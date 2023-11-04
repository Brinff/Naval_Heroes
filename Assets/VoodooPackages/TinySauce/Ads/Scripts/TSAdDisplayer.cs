using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Voodoo.Tiny.Sauce.Internal.Analytics;
using Random = UnityEngine.Random;

namespace Voodoo.Tiny.Sauce.Internal.Ads
{
    public class TSAdDisplayer : MonoBehaviour
    {
        [SerializeField] private TSAdImageBehaviour adImage;
        [SerializeField] private Button closeBtn;

        
        private static TSAdDisplayer _instance;
        public static TSAdDisplayer Instance { get => _instance; }
        
        private static TSAdData _currentAdData;
        private static TSFakeAdContent[] adList;

        
        private const string CLOSE_BTN_TXT = "X";
        private const float MIN_CLOSE_TIME_FS = 5;
        private const float MIN_CLOSE_TIME_RV = 30;

        
        private EventSystem eventSystemPrefab;
        private EventSystem eventSystem;

        private bool isAppPaused = false;
        private bool isFirstFrameAfterPause = false;
        
        private float gameTimeScale;

        private Text closeBtnText;
        private float minCloseTime;
        private float cooldown;
        
        public Action On_AdClosed;

        
        public static TSAdData CurrentAdData { set => _currentAdData = value; }
        

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            InitCloseBtn();
            InitAdList();
            InitEventSystem();
        }

        private void Start()
        {
            gameTimeScale = Time.timeScale;
            Time.timeScale = 0;

            SetupFakeAd();
        }

        private void Update()
        {
            if (cooldown > 0)
                UpdateCloseBtn();
            else
                EnableCloseBtn();
        }

        private void OnDestroy()
        {
            _instance = null;
            Time.timeScale = gameTimeScale;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            isAppPaused = !hasFocus;

            if (isAppPaused) isFirstFrameAfterPause = true;
        }


#region [ADS]
        private void InitAdList()
        {
            if (adList == null)
                adList = Resources.LoadAll<TSFakeAdContent>("FakeAds");
        }

        private void SetupFakeAd()
        {
            if (adList != null && adList.Length > 0)
            {
                _currentAdData.fakeAdContent = adList[Random.Range(0, adList.Length)].Load();
                InitAdInfo();
            }
            else
            {
                Debug.LogError("There are no fake ads in the 'Assets/VoodooPackages/TinySauce/Ads/Resources/FakeAds' folder");
                CloseAd();
            }
        }
        
        private void InitAdInfo()
        {
             adImage.CurrentAdData = _currentAdData;

            switch (_currentAdData.adType)
            {
                case TSAdType.Fake_Interstitial:
                    AnalyticsManager.TrackInterstitialShow(new AdShownEventAnalyticsInfo {
                        AdTag = _currentAdData.adType.ToString(),
                        AdNetworkName = _currentAdData.adSdkName,
                        adPlacement = _currentAdData.adPlacement,
                        GameCount = AnalyticsStorageHelper.GetGameCount()
                    });
                    break;
                case TSAdType.Fake_RewardedVideo:
                    AnalyticsManager.TrackRewardedShow(new AdShownEventAnalyticsInfo {
                        AdTag = _currentAdData.adType.ToString(),
                        AdNetworkName = _currentAdData.adSdkName,
                        adPlacement = _currentAdData.adPlacement,
                        GameCount = AnalyticsStorageHelper.GetGameCount()
                    });
                    break;
            }
            
            Debug.Log($"Pew! Pretending to send a GameAnalytics AdEvent (AdShown, AdType '{_currentAdData.adType.ToString()}', AdPlacement '{_currentAdData.adPlacement}') because it doesn't actually work in the editor");
        }
#endregion []

#region [CLOSE_BUTTON]
        private void InitCloseBtn()
        {
            closeBtnText = closeBtn.GetComponentInChildren<Text>();

            
            switch (_currentAdData.adType)
            {
                case TSAdType.Fake_Interstitial:
                    minCloseTime = MIN_CLOSE_TIME_FS;
                    break;
                case TSAdType.Fake_RewardedVideo:
                    minCloseTime = MIN_CLOSE_TIME_RV;
                    break;
            }

            closeBtn.enabled = false;
            cooldown = minCloseTime;
            closeBtnText.text = minCloseTime.ToString();
        }
        
        private void UpdateCloseBtn()
        {
            closeBtnText.text = Mathf.Ceil(cooldown).ToString();
            
            if (!isAppPaused)
            {
                if (isFirstFrameAfterPause)
                    isFirstFrameAfterPause = false;
                else
                    cooldown -= Time.unscaledDeltaTime;
            }
        }
        
        private void EnableCloseBtn()
        {
            closeBtnText.text = CLOSE_BTN_TXT;
            closeBtn.enabled = true;
        }
#endregion []

        
        private  void InitEventSystem()
        {
            if (FindObjectOfType<EventSystem>() != null) return;
            
            if (eventSystemPrefab == null)
                eventSystemPrefab = Resources.LoadAll<EventSystem>("Prefabs")[0];
                    
            if (eventSystemPrefab == null)
                Debug.LogError("There is no TSEventSystem prefab in the 'Assets/VoodooPackages/TinySauce/Resources/Prefabs' folder");

            eventSystem = Instantiate(eventSystemPrefab);
        }

        public void CloseAd()
        {
            if (eventSystem != null)
                Destroy(eventSystem.gameObject);
            
            On_AdClosed?.Invoke();
            Destroy(gameObject);
        }
    }
}
