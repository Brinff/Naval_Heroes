using UnityEngine;

namespace Code.Confings.Properties
{
    [System.Serializable]
    public class StringProperty : IConfigurableProperty
    {
        [SerializeField]
        private string m_Value;
        public string value => m_Value;
    }
}