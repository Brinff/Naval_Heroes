using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNotify : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_Group;
    [SerializeField]
    private RectTransform m_RectTransform;
    [SerializeField]
    private float m_RectTransformHeight = 100;
    [SerializeField]
    private DOTweenProperty m_RectTransformProperty = new DOTweenProperty();
    [SerializeField]
    private Image m_IconImage;
    [SerializeField]
    private DOTweenProperty m_IconImageProperty = new DOTweenProperty();
    [SerializeField]
    private Transform m_IconTransform;
    [SerializeField]
    private DOTweenProperty m_IconTransformProperty = new DOTweenProperty();
    [SerializeField]
    private RectTransform m_MaskTransform;
    [SerializeField]
    private DOTweenProperty m_MaskTransformProperty;
    [SerializeField]
    private float m_MaskTransformTargetSize;

    [Button]
    public Tween Show(PlayerNotificationWidget playerNotificationWidget)
    {
       var sequence = DOTween.Sequence(this);

        m_Group.alpha = 1;

        var color = m_IconImage.color;
        color.a = 0;
        m_IconImage.color = color;

        m_RectTransform.sizeDelta = new Vector2(m_RectTransform.sizeDelta.x, 0);
        sequence.Append(m_RectTransform.DOSizeDelta(new Vector2(m_RectTransform.sizeDelta.x, m_RectTransformHeight), m_RectTransformProperty.duration).SetEase(m_RectTransformProperty.ease).SetDelay(m_RectTransformProperty.delay).OnUpdate(playerNotificationWidget.Rebuild));

        sequence.Append(m_IconImage.DOFade(1, m_IconImageProperty.duration).SetEase(m_IconImageProperty.ease).SetDelay(m_IconImageProperty.delay));
        m_IconTransform.localScale = Vector3.one * 2;
        sequence.Append(m_IconTransform.DOScale(1, m_IconTransformProperty.duration).SetEase(m_IconTransformProperty.ease).SetDelay(m_IconTransformProperty.delay));

        m_MaskTransform.sizeDelta = new Vector2(0, m_MaskTransform.sizeDelta.y);
        sequence.Append(m_MaskTransform.DOSizeDelta(new Vector2(m_MaskTransformTargetSize, m_MaskTransform.sizeDelta.y), m_MaskTransformProperty.duration).SetEase(m_MaskTransformProperty.ease).SetDelay(m_MaskTransformProperty.delay));

        return sequence;
    }

    private Tween m_HideTween;

    [Button]
    public Tween Hide()
    {
        if (m_HideTween != null) m_HideTween.Kill();
        m_Group.alpha = 1;
        return m_HideTween = m_Group.DOFade(0, 0.3f);
    }

    private void OnDestroy()
    {
        if (m_HideTween != null) m_HideTween.Kill();
    }
}
