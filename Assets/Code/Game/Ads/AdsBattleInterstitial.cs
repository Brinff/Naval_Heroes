using Code.Ads;
using Code.IO;
using Code.Services;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Code.Game.Ads
{
    public class AdsBattleInterstitial : AdsInterstitial
    {
        [SerializeField]
        private int m_SecondsCondition;
        [SerializeField]
        private int m_CountAttemptCondition;
        [SerializeField]
        private int m_LevelOffsetCondition;

        private PlayerPrefsProperty<int> m_CountAttempt;
        private float m_PreviusTimeShow;

        public override void Initialize()
        {
            base.Initialize();
            m_CountAttempt = new PlayerPrefsProperty<int>(PlayerPrefsProperty.ToKey(nameof(AdsBattleInterstitial), nameof(m_CountAttempt)));
        }

        [Button]
        public override void Show()
        {
            PlayerMissionSystem playerMissionSystem = ServiceLocator.Get<EntityManager>().GetSystem<PlayerMissionSystem>();
            if (playerMissionSystem.level >= m_LevelOffsetCondition && Time.time - m_PreviusTimeShow > m_SecondsCondition)
            {
                if (m_CountAttempt <= 0)
                {
                    base.Show();
                    m_PreviusTimeShow = Time.time;
                    m_CountAttempt.value = m_CountAttemptCondition - 1;
                }
                else
                {
                    m_CountAttempt.value--;
                }
            }
        }
    }
}