using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using UnityEngine.UI;
using DG.Tweening;

public class ZoomToggleWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private Image m_ToggleImage;

    private bool m_IsToggle;

    public delegate void ToggleDelegate(bool value);

    public event ToggleDelegate OnToggle;

    public void SetToggle(bool isToggle)
    {
        m_IsToggle = isToggle;
        m_ToggleImage.DOFade(isToggle ? 1 : 0, 0.2f);
        m_Button.transform.DOScale(0.9f, 0.1f);
        m_Button.transform.DOScale(1f, 0.1f).SetDelay(0.1f);
    }

    private void OnEnable()
    {
        m_Button.onClick.AddListener(OnClickButton);
    }

    private void OnDisable()
    {
        m_Button.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {
        SetToggle(!m_IsToggle);
        OnToggle?.Invoke(m_IsToggle);
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
