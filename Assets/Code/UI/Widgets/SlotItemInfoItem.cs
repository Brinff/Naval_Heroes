using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemInfoItem : MonoBehaviour
{
    private static string[] s_Levels = new string[]
    {
        "I","II","III","IV","V","VI","VII","VIII","IX","X"
    };

    [SerializeField]
    private Image m_ClassificationAndRareImage;
    [SerializeField]
    private TextMeshProUGUI m_LevelLabel;

    private RectTransform m_View;
    private Camera m_WorldCamera;




    public void SetClassification(Sprite sprite)
    {
        m_ClassificationAndRareImage.sprite = sprite;
        m_ClassificationAndRareImage.SetNativeSize();
    }

    public void SetLevel(int level)
    {
        m_LevelLabel.text = s_Levels[level - 1];
    }

    public void SetRare(Color color)
    {
        m_ClassificationAndRareImage.color = color;
        m_LevelLabel.color = color;
    }

    public void Prepare(Camera camera, RectTransform view)
    {
        m_WorldCamera = camera;
        m_View = view;
    }
    private RectTransform m_RectTransform;
    private RectTransform rectTransform => m_RectTransform ? m_RectTransform : m_RectTransform = transform as RectTransform;

    public void UpdatePosition(Vector3 position)
    {
        if (m_WorldCamera == null) m_WorldCamera = Camera.main;

        float dot = Vector3.Dot(m_WorldCamera.transform.forward, Vector3.Normalize(position - m_WorldCamera.transform.position));
        rectTransform.gameObject.SetActive(dot > 0);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_View, m_WorldCamera.WorldToScreenPoint(position), null, out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }
}
