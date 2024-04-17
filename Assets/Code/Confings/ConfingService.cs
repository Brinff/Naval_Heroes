using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Sirenix.Utilities.Editor;
using UnityEditor;
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
        
        //[SerializeField, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
        //private Dictionary<string, IConfigurableProperty> m_Data = new Dictionary<string, IConfigurableProperty>();
        
        [Button]
        public void Save()
        {
            //Debug.Log(JsonConvert.SerializeObject(m_Data));
            var value = SerializationUtility.SerializeValue(m_Data, DataFormat.JSON);
            var text = Encoding.UTF8.GetString(value).Replace(" ", "").Replace("\n", "");
            Clipboard.Copy(text);
            //Debug.Log( );
            //Sirenix.Serialization.Serializer s = new AnySerializer(typeof(Data));
            //Stream stream = new MemoryStream();
            //var contex = new SerializationContext();
            //var jsonWriter = new JsonDataWriter(stream, contex);
            //s.WriteValueWeak(m_Data, jsonWriter);


            //Sirenix.Serialization.od
            //string jsonText = JsonUtility.ToJson(m_Data, true);
            //Debug.Log(jsonWriter.Stream.);
        }

        [Button]
        public void Load()
        {
            var bytes = Encoding.UTF8.GetBytes(m_Debug);
            m_Data = SerializationUtility.DeserializeValue<Data>(bytes, DataFormat.JSON);
        }
    }
}