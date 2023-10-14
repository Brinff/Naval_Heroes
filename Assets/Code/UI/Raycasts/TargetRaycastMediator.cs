using Game.Paterns;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TargetRaycastMediator : SerializedSingleton<TargetRaycastMediator>, ISingletonSetup, ITargetRaycaster
{
    [SerializeField, OnValueChanged("OnChange")]
    private bool m_IsOverrideTargetRaycasts;
    [SerializeField]
    private ITargetRaycaster[] m_Raycasters;

#if UNITY_EDITOR
    private void OnChange()
    {
        if (m_Raycasters != null)
        {
            foreach (var raycaster in m_Raycasters)
            {
                raycaster.isOverrideTargetRaycasts = m_IsOverrideTargetRaycasts;
            }
        }
    }
#endif

    public bool isOverrideTargetRaycasts
    {
        get => m_IsOverrideTargetRaycasts; 
        set
        {
            m_IsOverrideTargetRaycasts = value;
            if (m_Raycasters != null)
            {
                foreach (var raycaster in m_Raycasters)
                {
                    raycaster.isOverrideTargetRaycasts = value;
                }
            }
        }
    }

    [Button]
    public void AddTargetRaycast(GameObject gameObject)
    {
        if (m_Raycasters != null)
        {
            foreach (var raycaster in m_Raycasters)
            {
                raycaster.AddTargetRaycast(gameObject);
            }
        }
    }
    [Button]
    public void RemoveTargetRaycast(GameObject gameObject)
    {
        if (m_Raycasters != null)
        {
            foreach (var raycaster in m_Raycasters)
            {
                raycaster.RemoveTargetRaycast(gameObject);
            }
        }
    }

    public void Setup()
    {
        if (m_Raycasters != null)
        {
            foreach (var raycaster in m_Raycasters)
            {
                raycaster.isOverrideTargetRaycasts = m_IsOverrideTargetRaycasts;
            }
        }
    }
}
