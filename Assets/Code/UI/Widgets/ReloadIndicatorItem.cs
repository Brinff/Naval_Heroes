using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadIndicatorItem : MonoBehaviour
{
    [SerializeField]
    private Image m_FillImage;
    [SerializeField]
    private Color m_ReadyColor;
    [SerializeField]
    private Color m_ReloadColor;

    [SerializeField, Range(0, 1)]
    private float m_Progress;

    [SerializeField]
    private AnimationCurve m_ColorCurve = AnimationCurve.Linear(1, 0, 1, 2);
    [SerializeField]
    private AnimationCurve m_SizeCurve = AnimationCurve.Linear(1, 0, 1, 2);

    public void SetProgress(float progress)
    {
        m_Progress = Mathf.Clamp01(progress);
        m_FillImage.color = Color.Lerp(m_ReloadColor, m_ReadyColor, m_ColorCurve.Evaluate(m_Progress));
        m_FillImage.fillAmount = m_Progress;
        transform.localScale = Vector3.one * m_SizeCurve.Evaluate(m_Progress);
    }

//#if UNITY_EDITOR
//    private void OnDrawGizmos()
//    {
//        SetProgress(m_Progress);
//    }
//#endif
}
