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
    public void SetSprite(Sprite sprite)
    {
        m_Background.sprite = sprite;
        m_Fill.sprite = sprite;
    }



    public float reload => m_Fill.fillAmount;

    public void SetReload(float amount)
    {
        m_Fill.fillAmount = amount;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }
}
