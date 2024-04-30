using UnityEngine;

namespace Code.Confings.Properties
{
    [System.Serializable]
    public class Vector4Property : IConfigurableProperty
    {
        [SerializeField]
        private Vector4 m_Value;
        public Vector4 value => m_Value;
    }
}