using Game.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private Button m_RetryButtion;
    [SerializeField]
    private TextMeshProUGUI m_RewardLabel;

    private void OnEnable()
    {
        m_RetryButtion.onClick.AddListener(OnRetryClick);
    }

    private void OnDisable()
    {
        m_RetryButtion.onClick.RemoveListener(OnRetryClick);
    }

    public delegate void RetryDelegate();

    public event RetryDelegate OnRetry;

    private void OnRetryClick()
    {
        OnRetry?.Invoke();
    }

    public void SetReward(int amount)
    {
        m_RewardLabel.text = amount.ToString();
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
