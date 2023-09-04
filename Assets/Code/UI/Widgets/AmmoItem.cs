using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoItem : MonoBehaviour, IDisposable
{
    [SerializeField]
    private Image m_Background;
    [SerializeField]
    private Image m_Fill;
    [SerializeField]
    private Gradient m_ReloadColor;
    [SerializeField]
    private AnimationCurve m_SizeCurve;

    public void SetSprite(Sprite sprite)
    {
        m_Background.sprite = sprite;
        m_Fill.sprite = sprite;
    }


    private float m_Reload;
    public float reload => m_Reload;

    public void SetReload(float amount)
    {
        m_Reload = Mathf.Clamp01(amount);
        m_Fill.fillAmount = m_Reload;
        m_Fill.color = m_ReloadColor.Evaluate(amount);
        transform.localScale = Vector3.one * m_SizeCurve.Evaluate(amount);
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }
}
