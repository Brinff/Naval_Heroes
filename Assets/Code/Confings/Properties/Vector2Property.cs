using UnityEngine;

namespace Code.Confings.Properties
{
    [System.Serializable]
    public class Vector2Property : IConfigurableProperty
    {
        [SerializeField]
        private Vector2 m_Value;
        public Vector2 value => m_Value;
    }
}