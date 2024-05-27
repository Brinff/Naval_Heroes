using DG.Tweening;
using Newtonsoft.Json.Bson;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

namespace Code.UI.Components
{
    [System.Serializable, InlineProperty]
    public class TweenSequence
    {
        public enum AnimationType
        {
            None,
            Move, LocalMove,
            Rotate, LocalRotate,
            Scale,
            Color, Fade,
            Text,
            PunchPosition, PunchRotation, PunchScale,
            ShakePosition, ShakeRotation, ShakeScale,
            CameraAspect, CameraBackgroundColor, CameraFieldOfView, CameraOrthoSize, CameraPixelRect, CameraRect,
            UIWidthHeight, UIAnchorPosition, UIMin, UIMax, UIPivot
        }



        [System.Serializable]
        public class TweenData
        {
            [SerializeField, HorizontalGroup("Line 1"), HideLabel]
            private AnimationType m_AnimationType;
            [SerializeField, HorizontalGroup("Line 1"), HideLabel]
            private Component m_Target;


            [SerializeField, ShowIf(nameof(isShowEndVector3))]
            private Vector3 m_EndVector3;
            private bool isShowEndVector3 =>
                m_AnimationType == AnimationType.LocalMove ||
                m_AnimationType == AnimationType.Move ||
                m_AnimationType == AnimationType.Scale;

            [SerializeField, ShowIf(nameof(isShowEndVector2))]
            private Vector2 m_EndVector2;
            private bool isShowEndVector2 =>
                m_AnimationType == AnimationType.UIWidthHeight || m_AnimationType == AnimationType.UIAnchorPosition;

            [SerializeField, ShowIf(nameof(isShowEndQuaternion))]
            private Quaternion m_EndQuaternion;
            private bool isShowEndQuaternion =>
                m_AnimationType == AnimationType.LocalRotate ||
                m_AnimationType == AnimationType.Rotate;

            [SerializeField, ShowIf(nameof(isShowEndColor))]
            private Color m_EndColor;
            private bool isShowEndColor => m_AnimationType == AnimationType.Color;

            [SerializeField, ShowIf(nameof(isShowEndFloat))]
            private float m_EndFloat;
            private bool isShowEndFloat => m_AnimationType == AnimationType.Fade;


            [SerializeField, HorizontalGroup("Line 3")]
            private float m_Duration;
            [SerializeField, HorizontalGroup("Line 3")]
            private float m_Delay;
            [SerializeField, HorizontalGroup("Line 3")]
            private Ease m_Ease;

           

            public Tween Play()
            {
                return Create().SetEase(m_Ease).SetDelay(m_Delay);
            }

            public void Immedeately()
            {
                m_Target.DOKill();
                switch (m_AnimationType)
                {
                    case AnimationType.None: break;
                    case AnimationType.Move: m_Target.As<Transform>().position = m_EndVector3; break; //m_Target.As<Transform>().DOMove(m_EndVector3, m_Duration);
                    case AnimationType.LocalMove: m_Target.As<Transform>().localPosition = m_EndVector3; break; //.DOLocalMove(m_EndVector3, m_Duration);
                    case AnimationType.Rotate: m_Target.As<Transform>().rotation = m_EndQuaternion; break;
                    case AnimationType.LocalRotate: m_Target.As<Transform>().localRotation = m_EndQuaternion; break;
                    case AnimationType.Scale: m_Target.As<Transform>().localScale = m_EndVector3; break;
                    case AnimationType.Color: m_Target.As<Graphic>().color = m_EndColor; break;
                    case AnimationType.Fade: m_Target.As<CanvasGroup>().alpha = m_EndFloat; break;
                    case AnimationType.Text: break; //m_Target.As<Transform>().DOMove(m_EndVector3, m_Duration);
                    case AnimationType.PunchPosition: break; //m_Target.As<Transform>().DOPunchPosition(m_EndVector3, m_Duration);
                    case AnimationType.PunchRotation: break; //m_Target.As<Transform>().DOPunchRotation(m_EndVector3, m_Duration);
                    case AnimationType.PunchScale: break; //m_Target.As<Transform>().localScale = m_EndVector3;
                    case AnimationType.ShakePosition: break;
                    case AnimationType.ShakeRotation: break;
                    case AnimationType.ShakeScale: break;
                    case AnimationType.CameraAspect: break;
                    case AnimationType.CameraBackgroundColor: break;
                    case AnimationType.CameraFieldOfView: break;
                    case AnimationType.CameraOrthoSize: break;
                    case AnimationType.CameraPixelRect: break;
                    case AnimationType.CameraRect: break;
                    case AnimationType.UIWidthHeight: m_Target.As<RectTransform>().sizeDelta = m_EndVector2; break;
                    case AnimationType.UIAnchorPosition: m_Target.As<RectTransform>().anchoredPosition = m_EndVector2; break;
                    default: break;
                }
            }

            public Tween Create()
            {
                m_Target.DOKill();
                switch (m_AnimationType)
                {
                    case AnimationType.None: return null;
                    case AnimationType.Move: return m_Target.As<Transform>().DOMove(m_EndVector3, m_Duration);
                    case AnimationType.LocalMove: return m_Target.As<Transform>().DOLocalMove(m_EndVector3, m_Duration);
                    case AnimationType.Rotate: return m_Target.As<Transform>().DORotateQuaternion(m_EndQuaternion, m_Duration);
                    case AnimationType.LocalRotate: return m_Target.As<Transform>().DOLocalRotateQuaternion(m_EndQuaternion, m_Duration);
                    case AnimationType.Scale: return m_Target.As<Transform>().DOScale(m_EndVector3, m_Duration);
                    case AnimationType.Color: return m_Target.As<Graphic>().DOColor(m_EndColor, m_Duration);
                    case AnimationType.Fade: return m_Target.As<CanvasGroup>().DOFade(m_EndFloat, m_Duration);
                    case AnimationType.Text: return m_Target.As<Transform>().DOMove(m_EndVector3, m_Duration);
                    case AnimationType.PunchPosition: return m_Target.As<Transform>().DOPunchPosition(m_EndVector3, m_Duration);
                    case AnimationType.PunchRotation: return m_Target.As<Transform>().DOPunchRotation(m_EndVector3, m_Duration);
                    case AnimationType.PunchScale: return m_Target.As<Transform>().DOPunchScale(m_EndVector3, m_Duration);
                    case AnimationType.ShakePosition: return m_Target.As<Transform>().DOShakePosition(m_Duration);
                    case AnimationType.ShakeRotation: return m_Target.As<Transform>().DOShakeRotation(m_Duration);
                    case AnimationType.ShakeScale: return m_Target.As<Transform>().DOShakeScale(m_Duration);
                    case AnimationType.CameraAspect: return null;
                    case AnimationType.CameraBackgroundColor: return null;
                    case AnimationType.CameraFieldOfView: return null;
                    case AnimationType.CameraOrthoSize: return null;
                    case AnimationType.CameraPixelRect: return null;
                    case AnimationType.CameraRect: return null;
                    case AnimationType.UIWidthHeight: return m_Target.As<RectTransform>().DOSizeDelta(m_EndVector2, m_Duration);
                    case AnimationType.UIAnchorPosition: return m_Target.As<RectTransform>().DOAnchorPos(m_EndVector2, m_Duration);
                    default: return null;
                }
            }
        }

        [SerializeField]
        private List<TweenData> m_Tweens = new List<TweenData>();

        private bool isPlay => Application.isPlaying;

        [Button, EnableIf(nameof(isPlay))]
        public void Play()
        {
            Play(false);
        }

        public Tween Play(bool immedeately)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < m_Tweens.Count; i++)
            {
                var data = m_Tweens[i];
                sequence.Join(data.Play());
            }
            if(immedeately) sequence.Complete(true);
            return sequence;
        }
    }
}