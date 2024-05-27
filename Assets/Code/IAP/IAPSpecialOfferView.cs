using Code.Game.UI.Components;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.IAP
{
    public class IAPSpecialOfferView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TitleLabel;
        public TextMeshProUGUI titleLabel => m_TitleLabel;

        [SerializeField] private TextMeshProUGUI m_TimeLabel;
        public TextMeshProUGUI timeLabel => m_TimeLabel;
        
        [SerializeField] private TextMeshProUGUI m_CurrentCostLabel;
        public TextMeshProUGUI currentCostLabel => m_CurrentCostLabel;
        [SerializeField]private TextMeshProUGUI m_OldCostLabel;
        public TextMeshProUGUI oldCostLabel => m_OldCostLabel;
        
        [SerializeField] private ValueLabel m_RewardLabel;
        public ValueLabel rewardLabel => m_RewardLabel;
        [SerializeField] private TweenButton m_Button;
        public TweenButton button => m_Button;
    }
}