using DG.Tweening;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomViewWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private CanvasGroup m_Group;
    [SerializeField]
    private Material m_Material;

    [SerializeField]
    private List<ViewHole> m_Holes = new List<ViewHole>();

    [SerializeField]
    private Vector2 m_Paralax;

    [SerializeField, Range(0,1)]
    private float m_Zoom;
    [SerializeField, Range(0,1)]
    private float m_ZoomFactor;


    [System.Serializable]
    public class ViewHole
    {
        [SerializeField]
        private Image m_Image;
        private Material m_Material;
        [SerializeField]
        private RectTransform m_OuterRect;
        [SerializeField]
        private RectTransform m_InnerRect;
        [SerializeField]
        private Vector2 m_Paralax = new Vector2(0,0);
        [SerializeField]
        private AnimationCurve m_ZoomCurve = AnimationCurve.Linear(0,0,1,1);
        [SerializeField]
        private float m_ZoomFactorMin;
        [SerializeField]
        private float m_ZoomFactorMax;

        public void UpdateZoomFactor(float zoomFactor)
        {
            m_InnerRect.localScale = Vector3.one * Mathf.Lerp(m_ZoomFactorMin, m_ZoomFactorMax, zoomFactor);
        }

        public void UpdateZoom(float zoom)
        {
            m_OuterRect.localScale = Vector3.one * Mathf.Lerp(3, 1, m_ZoomCurve.Evaluate(zoom));
        }

        public void UpdateParalax(Vector2 paralax)
        {
            m_OuterRect.localPosition = Vector3.Scale(paralax, m_Paralax);
        }

        public void UpdateMaterial(Material material)
        {
            if (m_Material == null && m_Image != null)
            {
                m_Material = new Material(material);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
               
                
            }

            if (m_OuterRect && m_InnerRect)
            {
                Vector3 center = m_Image.rectTransform.InverseTransformPoint(m_OuterRect.position);
                Vector3 sizeInner = m_Image.rectTransform.InverseTransformVector(m_OuterRect.TransformVector(m_OuterRect.rect.size));
                Vector3 sizeOuter = m_Image.rectTransform.InverseTransformVector(m_InnerRect.TransformVector(m_InnerRect.rect.size));
                m_Material.SetVector("_Center", center);
                m_Material.SetFloat("_RadiusInner", sizeInner.x * 0.5f);
                m_Material.SetFloat("_RadiusOuter", sizeOuter.x * 0.5f);

                m_Image.material = m_Material;
            }
        }
    }



    private void Update()
    {
        m_Group.alpha = m_Zoom;
        foreach (var hole in m_Holes)
        {
            hole.UpdateZoomFactor(m_ZoomFactor);
            hole.UpdateZoom(m_Zoom);
            hole.UpdateParalax(m_Paralax);
            hole.UpdateMaterial(m_Material);
        }
    }

    public float GetZoom()
    {
        return m_Zoom;
    }

    public void SetZoom(float zoom)
    {
        m_Zoom = zoom;
    }

    public void SetZoomFactor(float zoomFactor)
    {
        m_ZoomFactor = zoomFactor;
    }

    [SerializeField]
    private float m_FactorParalax = 50f;
    [SerializeField]
    private float m_SmoothParalax = 1f;
    [SerializeField]
    private float m_MaxParalax = 50;
    private Vector2 m_ParalaxVelocity;

    public void SetDeltaParalax(Vector2 delta)
    {
        m_Paralax = Vector2.SmoothDamp(m_Paralax, delta * m_FactorParalax, ref m_ParalaxVelocity, m_SmoothParalax * Time.deltaTime);
        m_Paralax = Vector2.ClampMagnitude(m_Paralax, m_MaxParalax);
    }

    public void Hide(bool immediately)
    {
        if (m_ZoomTween != null) m_ZoomTween.Kill();
        
        if (immediately)
        {
            SetZoom(0);
            Update();
            gameObject.SetActive(false);
        }
        else
        {
            m_ZoomTween = DOTween.To(GetZoom, SetZoom, 0, 0.2f).OnComplete(OnCompleteHide);
        }
    }

    private void OnCompleteHide()
    {
        gameObject.SetActive(false);
    }

    private Tween m_ZoomTween;
    public void Show(bool immediately)
    {
        if (m_ZoomTween != null) m_ZoomTween.Kill();

        gameObject.SetActive(true);
        if (immediately)
        {
            SetZoom(1);
            Update();
        }
        else
        {
            m_ZoomTween = DOTween.To(GetZoom, SetZoom, 1, 0.3f);
        }
    }
}
