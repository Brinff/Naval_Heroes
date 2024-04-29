using Game.UI;
using System.Collections;
using System.Collections.Generic;
using Code.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Code.Game.UI.Components;

public class WinWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private ValueLabel m_RewardLabel;
    public ValueLabel rewardLabel => m_RewardLabel;

    [SerializeField]
    private ValueLabel m_CleonRewardLabel;
    public ValueLabel cleonRewardLabel => m_CleonRewardLabel;

    [SerializeField]
    private ValueLabel m_MissionLabel;
    public ValueLabel missionLabel => m_MissionLabel;
    [SerializeField]
    private Cleon m_Cleon;
    public Cleon cleon => m_Cleon;

    [SerializeField]
    private Button m_ClaimButton;
    [SerializeField]
    private Button m_NoThanksButton;

    public delegate void ClaimDelegate();
    public delegate void NoThanksDelegate();

    public event ClaimDelegate OnClaim;
    public event NoThanksDelegate OnNoThanks;

    private void OnEnable()
    {
        m_ClaimButton.onClick.AddListener(OnClaimClick);
        m_NoThanksButton.onClick.AddListener(OnNoThanksClick);
    }

    private void OnDisable()
    {
        m_ClaimButton.onClick.RemoveListener(OnClaimClick);
        m_NoThanksButton.onClick.RemoveListener(OnNoThanksClick);
    }

    private void OnClaimClick()
    {
        OnClaim?.Invoke();
    }

    private void OnNoThanksClick()
    {
        OnNoThanks?.Invoke();
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }
}
