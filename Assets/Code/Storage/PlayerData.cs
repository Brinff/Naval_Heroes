using System;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Reflection;
using Game;

public interface IDataResolver
{
    bool Resolve(string data, out string resolvedData);
}

public interface IPlayerPrefsData
{
    void Save();
    void Dispose();
}

[Serializable]
public class PlayerPrefsData<T> : IPlayerPrefsData where T : new()
{
    [SerializeField]
    private string m_Version;

    [SerializeField]
    private T m_Value;

    private string m_Key;
    public event Action<T> OnChange;
    private IDataResolver m_DataResolver;

    public PlayerPrefsData(string key, IDataResolver dataResolver)
    {
        m_Key = key;
        m_DataResolver = dataResolver;

        var s = PlayerPrefs.GetString(m_Key, "");
        if (m_DataResolver != null &&
            m_DataResolver.Resolve(s, out string rS))
        {
            Debug.Log($"Resolved from: {s} to: {rS}");
            s = rS;
        }
        if (string.IsNullOrEmpty(s)) m_Value = new T();
        else m_Value = JsonUtility.FromJson<PlayerPrefsData<T>>(s).m_Value;
    }

    public PlayerPrefsData(string key, T defaultValue = default)
    {
        m_Key = key;
        var s = PlayerPrefs.GetString(m_Key, "");
        if (string.IsNullOrEmpty(s)) m_Value = defaultValue;
        else m_Value = JsonUtility.FromJson<PlayerPrefsData<T>>(s).m_Value;
    }

    public T Value
    {
        get { return m_Value; }
        set
        {

            m_Value = value;
            Save();
        }
    }

    public string Version => m_Version;

    public static implicit operator T(PlayerPrefsData<T> d) => d.Value;

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

public class PlayerData : SerializedMonoBehaviour
{
    private static PlayerData _instance;
    public static PlayerData Instance
    {
        get
        {
            if (Application.isPlaying)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PlayerData>();
                    if (_instance != null)
                    {
                        _instance.Setup();
                    }
                    else
                    {
                        var go = new GameObject($"[{typeof(PlayerData)}]");
                        _instance = go.AddComponent<PlayerData>();
                    }
                }
            }
            return _instance;
        }
    }

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Setup();
        }
    }



    private bool IsActiveSave => Application.isPlaying;

    [Button(ButtonSizes.Medium), EnableIf("IsActiveSave")]
    public void SaveAll()
    {
        var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var value = field.GetValue(this) as IPlayerPrefsData;
            if (value != null) value.Save();
        }
    }

    [Button(ButtonSizes.Medium)]
    public void ClearAll()
    {
        //PlayerPrefs.DeleteAll();
        var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var value = field.GetValue(this) as IPlayerPrefsData;
            if (value != null) value.Dispose();
        }
    }


    public PlayerPrefsData<int> level;
    public PlayerPrefsData<int> softMoney;
    protected void Setup()
    {
        level = new PlayerPrefsData<int>(nameof(level), 1);
        softMoney = new PlayerPrefsData<int>(nameof(softMoney), 1);
        DontDestroyOnLoad(gameObject);
    }
}
