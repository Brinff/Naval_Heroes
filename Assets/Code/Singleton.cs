using UnityEngine;
using Sirenix.OdinInspector;

namespace Game.Paterns
{
    public interface ISingletonSetup
    {
        void Setup();
    }

    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {


        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance != null)
                    {
                        if (_instance is ISingletonSetup) (_instance as ISingletonSetup).Setup();
                    }
                    else
                    {
                        var go = new GameObject($"[{typeof(T)}]");
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                if (_instance is ISingletonSetup) (_instance as ISingletonSetup).Setup();
            }
        }
    }

    public abstract class SerializedSingleton<T> : SerializedMonoBehaviour where T : SerializedSingleton<T>
    {


        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance != null)
                    {
                        if (_instance is ISingletonSetup) (_instance as ISingletonSetup).Setup();
                    }
                    else
                    {
                        var go = new GameObject($"[{typeof(T)}]");
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                if (_instance is ISingletonSetup) (_instance as ISingletonSetup).Setup();
            }
        }
    }
}