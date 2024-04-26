using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Text;
using UnityEngine;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

namespace Code.Confings
{
    public class ConfingService : SerializedMonoBehaviour
    {
        [SerializeField, Multiline]
        private string m_Debug;
        
        [System.Serializable]
        public class PropertyPair
        {
            [OdinSerialize]
            public string m_Key;
            [OdinSerialize]
            public IConfigurableProperty m_Property;
        }
        
        [System.Serializable]
        private class Data
        {
            [OdinSerialize]
            private List<PropertyPair> m_PropertyPairs = new List<PropertyPair>();
        }

        [OdinSerialize]
        private Data m_Data = new Data();

        [Button]
        public void Save()
        {
            var value = SerializationUtility.SerializeValue(m_Data, DataFormat.JSON);
            var text = Encoding.UTF8.GetString(value).Replace(" ", "").Replace("\n", "");
        }

        [Button]
        public void Load()
        {
            var bytes = Encoding.UTF8.GetBytes(m_Debug);
            m_Data = SerializationUtility.DeserializeValue<Data>(bytes, DataFormat.JSON);
        }
    }
}