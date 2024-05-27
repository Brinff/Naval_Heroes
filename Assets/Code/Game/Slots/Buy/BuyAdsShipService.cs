
using Code.Ads;
using Code.IO;
using Code.Services;
using UnityEngine;

namespace Code.Game.Slots.Buy
{
    public class BuyAdsShipService : MonoBehaviour, IService, IInitializable
    {
        [SerializeField]
        private Category m_Category;
        public Category category => m_Category;

        private AdsReward m_AdsReward;


        private PlayerPrefsProperty<int> m_CountInStash;

        public int countInStash => m_CountInStash.value;


        public delegate void UdpateDelegate();
        public event UdpateDelegate OnUpdate;

        public delegate void DoneDelegate(bool isDone);
        public event DoneDelegate OnDone;

        private bool m_IsDirty;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }
        private bool m_IsInitialized;
        public void Initialize()
        {
            m_AdsReward = ServiceLocator.Get<AdsReward>();
            m_AdsReward.OnUpdate += OnAdsUpdate;

            m_CountInStash = new PlayerPrefsProperty<int>(PlayerPrefsProperty.ToKey(nameof(BuyAdsShipService), m_Category.name, nameof(m_CountInStash))).Build();
            m_IsInitialized = true;
        }

        public bool Spend()
        {
            if (m_CountInStash.value > 0)
            {
                m_CountInStash.value--;
                m_IsDirty = true;
                return true;
            }
            return false;
        }

        public bool IsReady()
        {
            return m_AdsReward.IsReady();
        }

        public bool Show()
        {
            return m_AdsReward.Show($"buy_slot_{m_Category.name}", Done);
        }

        private void OnAdsUpdate()
        {
            m_IsDirty = true;
        }

        private void Done(bool value)
        {
            if (value)
            {
                m_CountInStash.value += 2;
                m_IsDirty = true;
            }
            OnDone?.Invoke(value);
        }

        private void Update()
        {
            if (!m_IsInitialized) return;
            if (m_IsDirty)
            {
                OnUpdate?.Invoke();
                m_IsDirty = true;
            }
        }

        public bool isEmpty => countInStash == 0;
    }
}