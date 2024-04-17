using UnityEngine;

namespace Code.Confings.Properties
{
    [System.Serializable]
    public class BoolProperty : IConfigurableProperty
    {
        [SerializeField]
        private bool m_Value;
        public bool value => m_Value;
    }
}