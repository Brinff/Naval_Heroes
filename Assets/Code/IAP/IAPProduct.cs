using System.Collections;
using UnityEngine;

namespace Code.IAP
{
    public class IAPProduct : ScriptableObject
    {
        [SerializeField]
        private string m_Id;
        public string id => m_Id;
    }
}