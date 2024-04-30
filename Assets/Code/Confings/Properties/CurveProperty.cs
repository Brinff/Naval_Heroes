using UnityEngine;

namespace Code.Confings.Properties
{
    [System.Serializable]
    public class CurveProperty : IConfigurableProperty
    {
        [SerializeField]
        private AnimationCurve m_Value;
        public AnimationCurve value => m_Value;
    }
}