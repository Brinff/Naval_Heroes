using Code.Game.UI.Components;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.IAP
{
    public class IAPMoneyPackView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TitleLabel;
        public TextMeshProUGUI titleLabel => m_TitleLabel;
        
        [SerializeField] private TextMeshProUGUI m_CostLabel;
        [SerializeField] private Image m_IconImage;
        public Image iconImage => m_IconImage;
        public TextMeshProUGUI costLabel => m_CostLabel;
        [SerializeField] private ValueLabel m_RewardLabel;
        public ValueLabel rewardLabel => m_RewardLabel;
        [SerializeField] private ValueLabel m_ValueLabel;
        public ValueLabel valueLabel => m_ValueLabel;
        [SerializeField] private TweenButton m_Button;
        public TweenButton button => m_Button;
    }
}