using DG.Tweening;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private TextMeshProUGUI m_Label;
    [SerializeField]
    private CanvasGroup m_Group;
    private Tween m_Tween;
    public void Hide(bool immediately)
    {
        if (m_Tween != null) m_Tween.Kill();
        if (immediately)
        {
            m_Group.alpha = 0;
            gameObject.SetActive(false);
        }
        m_Tween = m_Group.DOFade(0, 0.3f).OnComplete(OnCompleteHide);
    }

    private void OnCompleteHide()
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        if (m_Tween != null) m_Tween.Kill();
        gameObject.SetActive(true);
        m_Group.DOFade(1, 0.3f);
    }

    public void SetText(string text)
    {
        m_Label.text = text;
    }
}
