using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using TMPro;
using Warships;
using Game;
using UnityEngine.UI;
using DG.Tweening;

public class MovementInputButtonsWidget : MonoBehaviour, IUIElement
{
    public delegate void MovementInputDelegate(Vector2 axis);

    public enum ButtonMode { Incresse, Reset }





    [SerializeField]
    private TextMeshProUGUI m_SpeedLabel = null;

    [SerializeField]
    private ButtonMode m_ForwardButtonMode;
    [SerializeField]
    private int m_CountStepForward = 2;
    [SerializeField]
    private Image[] m_ForwardArrow;

    [SerializeField]
    private float m_SpeedChangeRightLeft = 1;
    [SerializeField]
    private float m_SpeedResetRightLeft = 2;

    [SerializeField]
    private ButtonMode m_BackButtonMode;
    [SerializeField]
    private int m_CountStepBackward = 2;
    [SerializeField]
    private Image[] m_BackwardArrow;


    [SerializeField, ReadOnly]
    private Vector2 m_Axis = Vector2.zero;
    private Vector2 m_CheckAxis = Vector2.zero;
    [ShowInInspector, ReadOnly]
    private MovementInputButton.Direction m_PerformDirection = MovementInputButton.Direction.None;

    public MovementInputDelegate OnChangeAxisValue;

    public void Show(bool immediately)
    {
        UpdateArrow(m_Axis);
        gameObject.SetActive(true);
    }

    public void Hide(bool immediately)
    {
        m_PerformDirection = MovementInputButton.Direction.None;
        m_Axis = Vector2.zero;
        OnChangeAxisValue?.Invoke(m_Axis);

        gameObject.SetActive(false);
    }

    public void SetSpeed(float speed, SpeedUnit speedUnit)
    {
        string units = "M/s";
        switch (speedUnit)
        {
            case SpeedUnit.KNOTS_PER_H:
                units = "Knots";
                break;
            case SpeedUnit.KM_PER_H:
                units = "Km/h";
                break;
            case SpeedUnit.M_PER_H:
                units = "M/h";
                break;
        }
        m_SpeedLabel.text = $"{(int)speed} {units}";
    }

    public void OnButtonDown(MovementInputButton.Direction direction)
    {
        m_PerformDirection |= direction;
    }

    public void OnButtonUp(MovementInputButton.Direction direction)
    {


        if (m_PerformDirection.HasFlag(MovementInputButton.Direction.Forward))
        {
            if (m_Axis.y > 0 && m_ForwardButtonMode == ButtonMode.Reset) m_Axis.y = 0;
            else
            {
                if (m_Axis.y < 0) m_Axis.y += 1f / m_CountStepBackward;
                else m_Axis.y += 1f / m_CountStepForward;
            }
        }
        if (m_PerformDirection.HasFlag(MovementInputButton.Direction.Backward))
        {
            if (m_Axis.y > 0 && m_BackButtonMode == ButtonMode.Reset) m_Axis.y = 0;
            else
            {
                if (m_Axis.y > 0) m_Axis.y -= 1f / m_CountStepForward;
                else m_Axis.y -= 1f / m_CountStepBackward;
            }
        }

        m_PerformDirection &= ~direction;

        m_Axis.x = Mathf.Clamp(m_Axis.x, -1, 1);
        m_Axis.y = Mathf.Clamp(m_Axis.y, -1, 1);

        UpdateArrow(m_Axis);
    }

    private void UpdateArrow(Vector2 axis)
    {

        int forwardGear = -1;
        if (axis.y > 0)
        {
            forwardGear = Mathf.RoundToInt(Mathf.Abs(axis.y) * (m_CountStepForward - 1));
        }

        int backwardGear = -1;
        if (axis.y < 0)
        {
            backwardGear = Mathf.RoundToInt(Mathf.Abs(axis.y) * (m_CountStepBackward - 1));
        }

        for (int i = 0; i < m_ForwardArrow.Length; i++)
        {
            bool isAcitve = forwardGear == i;
            m_ForwardArrow[i].DOFade(isAcitve ? 1 : 0, 0.2f);
        }

        for (int i = 0; i < m_BackwardArrow.Length; i++)
        {
            bool isAcitve = backwardGear == i;
            m_BackwardArrow[i].DOFade(isAcitve ? 1 : 0, 0.2f);
        }
    }


    private void Update()
    {

#if UNITY_EDITOR
        Keyboard();
#endif

        if (m_PerformDirection.HasFlag(MovementInputButton.Direction.Left)) m_Axis.x -= m_SpeedChangeRightLeft * Time.deltaTime;
        if (m_PerformDirection.HasFlag(MovementInputButton.Direction.Right)) m_Axis.x += m_SpeedChangeRightLeft * Time.deltaTime;

        if ((!m_PerformDirection.HasFlag(MovementInputButton.Direction.Left) && !m_PerformDirection.HasFlag(MovementInputButton.Direction.Right)) && Mathf.Abs(m_Axis.x) > 0)
        {
            m_Axis.x += -Mathf.Sign(m_Axis.x) * m_SpeedResetRightLeft * Time.deltaTime;
            if (Mathf.Abs(m_Axis.x) < Time.deltaTime) m_Axis.x = 0;
        }

        if (m_CheckAxis != m_Axis)
        {
            
            m_CheckAxis = m_Axis;
            OnChangeAxisValue?.Invoke(m_Axis);
        }
    }

#if UNITY_EDITOR
    private void Keyboard()
    {
        if (Input.GetKeyDown(KeyCode.A)) OnButtonDown(MovementInputButton.Direction.Left);
        if (Input.GetKeyDown(KeyCode.D)) OnButtonDown(MovementInputButton.Direction.Right);
        if (Input.GetKeyDown(KeyCode.W)) OnButtonDown(MovementInputButton.Direction.Forward);
        if (Input.GetKeyDown(KeyCode.S)) OnButtonDown(MovementInputButton.Direction.Backward);

        if (Input.GetKeyUp(KeyCode.A)) OnButtonUp(MovementInputButton.Direction.Left);
        if (Input.GetKeyUp(KeyCode.D)) OnButtonUp(MovementInputButton.Direction.Right);
        if (Input.GetKeyUp(KeyCode.W)) OnButtonUp(MovementInputButton.Direction.Forward);
        if (Input.GetKeyUp(KeyCode.S)) OnButtonUp(MovementInputButton.Direction.Backward);
    }
#endif

}
