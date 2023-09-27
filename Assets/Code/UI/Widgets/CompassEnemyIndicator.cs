using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CompassEnemyIndicator : MonoBehaviour
{
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private Color m_FromShowColor;
    private RectTransform m_RectTransform;

    public RectTransform rectTransform => m_RectTransform ? m_RectTransform : m_RectTransform = GetComponent<RectTransform>();

    [Button]
    public void Show()
    {
        m_Image.color = m_FromShowColor;
        transform.localScale = Vector3.one * 2f;
        m_Image.DOColor(Color.white, 0.3f);
        transform.DOScale(1, 1f).SetEase(Ease.OutBack);
    }

    //[Button]
    //public void Hide(bool immediately)
    //{
    //    //if (immediately)
    //    //{
    //        Destroy(gameObject);
    //    //}
    //    //else m_Image.DOFade(0, 0.2f).OnComplete(Kill);
    //}

    //private void Kill()
    //{
    //    Destroy(gameObject, 0.5f);
    //}
}
