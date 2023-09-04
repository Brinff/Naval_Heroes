using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using UnityEngine.UI;

public class AutoFightToggleWidget : MonoBehaviour, IUIElement
{
    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public delegate void ToggleDelegate(bool isToggle);

    public event ToggleDelegate OnToggle;

    private bool m_IsToggle;
    [SerializeField]
    private GameObject m_Checkmark;

    public bool isToggle { get { return m_IsToggle; } set { m_IsToggle = value; m_Checkmark.SetActive(m_IsToggle); } }

    [SerializeField]
    private Button m_Button;

    private void OnEnable()
    {
        m_Button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        m_Button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        OnToggle?.Invoke(!m_IsToggle);
    }
}
