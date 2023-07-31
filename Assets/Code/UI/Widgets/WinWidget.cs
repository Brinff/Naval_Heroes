using Game.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private TextMeshProUGUI m_RewardLabel;
    [SerializeField]
    private TextMeshProUGUI m_MissionLabel;
    [SerializeField]
    private Button m_ClaimButton;

    public delegate void ClaimDelegate();

    public event ClaimDelegate OnClaim;

    private void OnEnable()
    {
        m_ClaimButton.onClick.AddListener(OnClaimClick);
    }

    private void OnDisable()
    {
        m_ClaimButton.onClick.RemoveListener(OnClaimClick);
    }

    private void OnClaimClick()
    {
        OnClaim?.Invoke();
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public void SetLevel(int level)
    {
        m_MissionLabel.text = $"Mission {level} complete!";
    }

    public void SetReward(int amount, bool immediately)
    {
        m_RewardLabel.text = amount.KiloFormat();
    }
}
