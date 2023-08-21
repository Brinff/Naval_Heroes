using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_Group;
    [SerializeField]
    private Image m_BarFillCurrent;
    [SerializeField]
    private float m_BarFillCurrentDuration = 0.1f;
    [SerializeField]
    private Ease m_BarFillCurrentEase = Ease.Linear;

    [SerializeField]
    private Image m_BarFillDelayer;
    [SerializeField]
    private float m_BarFillDelayerDuration = 0.1f;
    [SerializeField]
    private Ease m_BarFillDelayerEase = Ease.Linear;

    private Tween m_FillDelayer;
    private Tween m_FillCurrent;

    [SerializeField]
    private Image m_ClassificationAndRareImage;

    [SerializeField]
    private TextMeshProUGUI m_Label;

    private float m_NormalizeValue;

    private RectTransform m_RectTransform;

    public RectTransform rectTransform => m_RectTransform ? m_RectTransform : m_RectTransform = GetComponent<RectTransform>();

    public void SetClassification(Sprite sprite)
    {
        m_ClassificationAndRareImage.sprite = sprite;
        m_ClassificationAndRareImage.SetNativeSize();
    }

    public void SetRare(Color color)
    {
        m_ClassificationAndRareImage.color = color;
    }

    public void SetHealth(float health, float maxHealth)
    {
        m_NormalizeValue = Mathf.Clamp01(health / maxHealth);

        if (m_FillCurrent != null) m_FillCurrent.Kill();
        if (m_FillDelayer != null) m_FillDelayer.Kill();


        if (m_BarFillCurrent) m_BarFillCurrent.fillAmount = m_NormalizeValue;
        if (m_BarFillDelayer) m_BarFillDelayer.fillAmount = m_NormalizeValue;

        if (m_Label) m_Label.text = $"{Mathf.FloorToInt(m_NormalizeValue * 100)} %";
    }

    public void DoHeath(float health, float maxHealth)
    {
        float n = Mathf.Clamp01(health / maxHealth);

        if (m_FillCurrent != null) m_FillCurrent.Kill();
        if (m_FillDelayer != null) m_FillDelayer.Kill();

        if (m_BarFillCurrent) m_FillCurrent = DOTween.To(GetHealth, SetHealth, n, m_BarFillCurrentDuration).SetEase(m_BarFillCurrentEase);
        if (m_BarFillDelayer) m_FillDelayer = m_BarFillDelayer.DOFillAmount(n, m_BarFillDelayerDuration).SetEase(m_BarFillDelayerEase);
    }

    private float GetHealth()
    {
        return m_NormalizeValue;
    }

    private void SetHealth(float n)
    {
        m_NormalizeValue = n;
        if (m_Label) m_Label.text = $"{Mathf.FloorToInt(m_NormalizeValue * 100)} %";
        m_BarFillCurrent.fillAmount = n;
    }

    private Tween m_GroupTween;

    public void WaitOnEndHealth(TweenCallback onEndHealth)
    {
        if (m_FillDelayer != null)
        {
            if (!m_FillDelayer.IsComplete()) m_FillDelayer.OnComplete(onEndHealth);
            else onEndHealth.Invoke();
        }
        else onEndHealth.Invoke();
    }

    public Tween DoShow(bool immediately)
    {
        if (m_GroupTween != null) m_GroupTween.Kill();
        if (immediately)
        {
            m_Group.alpha = 1;
            return null;
        }
        return m_GroupTween = m_Group.DOFade(1, 0.3f);
    }

    public Tween DoHide(bool immediately)
    {
        if (m_GroupTween != null) m_GroupTween.Kill();
        if (immediately)
        {
            m_Group.alpha = 0;
            return null;
        }
        return m_GroupTween = m_Group.DOFade(0, 0.3f);
    }
}
