using UnityEngine;

namespace Code.Confings.Properties
{
    [System.Serializable]
    public class Vector3Property : IConfigurableProperty
    {
        [SerializeField]
        private Vector3 m_Value;
        public Vector3 value => m_Value;
    }
}