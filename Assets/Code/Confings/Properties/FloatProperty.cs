using UnityEngine;

namespace Code.Confings.Properties
{
    [System.Serializable]
    public class FloatProperty : IConfigurableProperty
    {
        [SerializeField]
        private float m_Value;
        public float value => m_Value;
    }
}