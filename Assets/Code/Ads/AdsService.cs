using Code.Services;
using System.Collections;
using UnityEngine;

namespace Code.Ads
{
    public class AdsService : MonoBehaviour, IService, IInitializable
    {
        [SerializeField, TextArea]
        private string m_Key;

        public void Initialize()
        {
            Debug.Log("AppLovin: Begin Initialize");
            MaxSdkCallbacks.OnSdkInitializedEvent += OnInitialized;
            MaxSdk.SetSdkKey(m_Key);
            MaxSdk.InitializeSdk();
        }

        private void OnInitialized(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            Debug.Log("AppLovin: End Initialized");
        }
    }
}