using System;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.IO
{
    public interface IDataResolver
    {
        bool Resolve(string data, out string resolvedData);
    }

    public interface IPlayerPrefsProperty
    {
        void Save();
        void Dispose();
    }

    [Serializable]
    public class PlayerPrefsProperty<T> : IPlayerPrefsProperty where T : new()
    {
        [SerializeField]
        private string m_Version;

        [SerializeField]
        private T m_Value;

        private string m_Key;
        public event Action<T> OnChange;
        private IDataResolver m_DataResolver;

        /*public PlayerPrefsProperty(string key, IDataResolver dataResolver)
        {
            m_Key = ToKey(key);
            m_DataResolver = dataResolver;

            var s = PlayerPrefs.GetString(m_Key, "");
            if (m_DataResolver != null &&
                m_DataResolver.Resolve(s, out string rS))
            {
                Debug.Log($"Resolved from: {s} to: {rS}");
                s = rS;
            }
            if (string.IsNullOrEmpty(s)) m_Value = new T();
            else m_Value = JsonUtility.FromJson<PlayerPrefsProperty<T>>(s).m_Value;
        }*/

        public static string ToKey(string key)
        {
            return Regex.Replace(key, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1_").ToUpper();
        }


        public delegate T DefaultDelegate();
        private DefaultDelegate m_DefaultDelegate;
        public PlayerPrefsProperty<T> OnDefault(DefaultDelegate onDefault)
        {
            m_DefaultDelegate = onDefault;
            return this;
        }

        public PlayerPrefsProperty<T> Build()
        {
            var s = PlayerPrefs.GetString(m_Key, "");
            if (string.IsNullOrEmpty(s))
            {
                m_Value = m_DefaultDelegate != null ? m_DefaultDelegate.Invoke() : default(T);
            }
            else m_Value = JsonUtility.FromJson<PlayerPrefsProperty<T>>(s).m_Value;
            return this;
        }
        
        public PlayerPrefsProperty(string key)
        {
            m_Key = key;
        }

        public T value
        {
            get { return m_Value; }
            set
            {

                m_Value = value;
                Save();
            }
        }

        public string version => m_Version;

        public static implicit operator T(PlayerPrefsProperty<T> d) => d.value;

        [Button]
        public void Save()
        {
            m_Version = Application.version;
            Debug.Log($"Save:  v. {m_Version} {m_Key} = {m_Value}");
            PlayerPrefs.SetString(m_Key, JsonUtility.ToJson(this));
            OnChange?.Invoke(m_Value);
        }

        public void Dispose()
        {
            m_Value = default(T);
            PlayerPrefs.SetString(m_Key, "");
        }
    }
}