using System;
using System.Globalization;
using Code.Utility;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Code.Game.UI.Components
{
    public class ValueLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Label;

        [SerializeField]
        private string m_LineFormat;
        
        public string lineFormat
        {
            get => m_LineFormat;
            set => m_LineFormat = value;
        }
        
        [SerializeField] private string m_ValueFormat;
        
        public enum ValueView
        {
            Number, 
            Kilo, 
            Abc,
            Percentage
        }
        
        [SerializeField] private ValueView m_ValueView;
        
        public ValueView valueView
        {
            get => m_ValueView;
            set => m_ValueView = value;
        }
        
        private string m_Text;


        private string GetText(int value)
        {
            string textValue = string.IsNullOrEmpty(m_ValueFormat) ? value.ToString() : value.ToString(m_ValueFormat);
            if (m_ValueView == ValueView.Kilo) textValue = value.KiloFormatShort();
            if (m_ValueView == ValueView.Abc) textValue = value.ToAbc();
            if (m_ValueView == ValueView.Percentage) textValue = $"{value}%";
            //string textValue = m_ValueView ? value.KiloFormatShort() : (string.IsNullOrEmpty(m_ValueFormat) ? value.ToString() : value.ToString(m_ValueFormat));
            return string.IsNullOrEmpty(m_LineFormat) ? textValue : String.Format(m_LineFormat, textValue);
        }
        
        private string GetText(float value)
        {
            var culture = CultureInfo.InvariantCulture;
            
            string textValue = string.IsNullOrEmpty(m_ValueFormat) ? value.ToString() : value.ToString(m_ValueFormat);
            if (m_ValueView == ValueView.Kilo) textValue = value.KiloFormat();
            if (m_ValueView == ValueView.Abc) textValue = value.ToAbc();
            if (m_ValueView == ValueView.Percentage) textValue = $"{(value * 100).ToString("0.##", culture)}%";
            //string textValue = m_ValueView ? value.KiloFormatShort() : (string.IsNullOrEmpty(m_ValueFormat) ? value.ToString() : value.ToString(m_ValueFormat));
            return string.IsNullOrEmpty(m_LineFormat) ? textValue : String.Format(m_LineFormat, textValue);
        }

        private Tween m_ScaleTween;

        private float m_FloatValue;
        private int m_IntValue;
        private int GetIntValue()
        {
            return m_IntValue;
        }

        private void SetIntValue(int value)
        {
            SetValue(value, false);
        }
        
        public Tween DoValue(int value, float duration)
        {
            var tween = DOTween.To(GetIntValue, SetIntValue, value, duration).SetTarget(this);
            return tween;
        }
        
        [Button]
        public void SetValue(int value, bool immediately)
        {
            m_IntValue = value;
            var text = GetText(value);
            if (m_Text != text)
            {
                m_ScaleTween?.Kill();
                m_Text = text;
                m_Label.text = m_Text;
                if (!immediately)
                {
                    transform.localScale = Vector3.one;
                    m_ScaleTween = transform.DOScale(1.2f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
                }
            }
        }
        
        [Button]
        public void SetValue(float value, bool immediately)
        {
            m_FloatValue = value;
            var text = GetText(value);
            if (m_Text != text)
            {
                m_ScaleTween?.Kill();
                m_Text = text;
                m_Label.text = m_Text;
                if (!immediately)
                {
                    transform.localScale = Vector3.one;
                    m_ScaleTween = transform.DOScale(1.2f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
                }
            }
        }
    }
}