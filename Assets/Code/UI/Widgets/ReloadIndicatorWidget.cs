using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadIndicatorWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private ReloadIndicatorItem m_Prefab;
    [SerializeField]
    private RectTransform m_Root;

    public ReloadIndicatorItem CreateIndicator(float progress)
    {
        var indicator = Instantiate(m_Prefab, m_Root);
        indicator.gameObject.SetActive(true);
        indicator.SetProgress(progress);
        return indicator;
    }

    public void DestoryIndicator(ReloadIndicatorItem indicator)
    {
        Destroy(indicator.gameObject);
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        m_Prefab.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
