using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDebugArrow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private SlotDebug m_SlotDebug;
    [SerializeField]
    private int m_Offset;
    public void OnPointerClick(PointerEventData eventData)
    {
        m_SlotDebug.MoveSelect(m_Offset);
    }

}
