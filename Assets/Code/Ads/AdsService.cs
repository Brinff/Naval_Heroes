using Assets.Code.Ads;
using Code.Services;
using System.Collections;
using UnityEngine;

namespace Code.Ads
{
    public class AdsService : MonoBehaviour, IService, IInitializable
    {
        [SerializeField, TextArea]
        private string m_Key;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {

            ServiceLocator.Unregister(this);
        }

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
            ServiceLocator.ForEach<IAdsService>(x => x.As<IInitializable>()?.Initialize());
        }
    }
}