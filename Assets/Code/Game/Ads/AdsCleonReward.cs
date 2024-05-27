using Code.Ads;
using Code.Services;
using System.Collections;
using UnityEngine;

namespace Code.Game.Ads
{
    public class AdsCleonReward : MonoBehaviour, IAdsService, IInitializable
    {
        [SerializeField]
        private string m_Placement;

        private AdsReward m_AdsReward;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public bool Show(System.Action<bool> onDone)
        {
            return m_AdsReward.Show(m_Placement, onDone);
        }

        public void Initialize()
        {
            m_AdsReward = ServiceLocator.Get<AdsReward>();
        }
    }
}