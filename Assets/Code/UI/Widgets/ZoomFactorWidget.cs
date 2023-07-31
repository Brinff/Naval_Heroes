using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using UnityEngine.UI;

public class ZoomFactorWidget : MonoBehaviour, IUIElement
{
    public delegate void ZoomValueDelegate(float value);

    public event ZoomValueDelegate OnChangeZoomFactor;

    [SerializeField]
    private Slider m_Slider;

    public void SetValue(float value)
    {
        m_Slider.value = value;
    }

    private void OnEnable()
    {
        m_Slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        m_Slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        OnChangeZoomFactor?.Invoke(value);
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
