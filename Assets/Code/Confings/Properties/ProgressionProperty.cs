using UnityEngine;

namespace Code.Confings.Properties
{
    [System.Serializable]
    public class ProgressionProperty : IConfigurableProperty
    {
        [SerializeField]
        private float m_Start;
        [SerializeField]
        private float m_Pow;
        [SerializeField]
        private float m_Factor;
    }
}