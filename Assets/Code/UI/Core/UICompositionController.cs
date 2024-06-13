using Code.Services;
using Game.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICompositionController : MonoBehaviour, IService
{
    [SerializeField]
    private UIComposition m_DefaultComposition;

    [SerializeField]
    private List<UIComposition> m_Compositions = new List<UIComposition>();

    private List<IUIElement> m_Elements = new List<IUIElement>();
    private List<IUIElement> m_ShowedElements = new List<IUIElement>();

    private void OnEnable()
    {
        ServiceLocator.Register(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unregister(this);
    }

	private void Awake()
	{
		m_Compositions.ForEach(c => c.ElementRemoved += OnElementRemoved);
	}

	private void OnDestroy()
	{
		m_Compositions.ForEach(c => c.ElementRemoved -= OnElementRemoved);
	}

	public void Register(UIComposition composition)
    {
        if (!m_Compositions.Contains(composition))
        {
            var elements = composition.GetElements();

            foreach (var element in elements)
            {
                if (!m_Elements.Contains(element)) m_Elements.Add(element);
            }

            m_Compositions.Add(composition);

			composition.ElementRemoved += OnElementRemoved;
        }
    }

	private void OnElementRemoved(UIComposition.Element element)
	{
        print($"removing {element}, result: {m_ShowedElements.Remove(element.element as IUIElement)}");
	}

	public void Unregister(UIComposition composition)
    {
        m_Compositions.Remove(composition);
		composition.ElementRemoved -= OnElementRemoved;
	}

    [Button]
    public void Hide()
    {
        for (int i = 0; i < m_ShowedElements.Count; i++)
        {
            var se = m_ShowedElements[i];
            if (se != null)
            {
                se.Hide(true);
            }
        }

        m_ShowedElements.Clear();
    }


    public void Show(Type type, bool immediately = false)
    {
        var composition = m_Compositions.Find(x => x.GetType() == type);
        if (composition != null)
        {
            var elements = composition.GetElements();
            for (int i = 0; i < m_ShowedElements.Count; i++)
            {
                var se = m_ShowedElements[i];
                if (!elements.Contains(se))
                {
                    if (se != null)
                    {
                        se.Hide(immediately);
                    }
                    m_ShowedElements.Remove(se);
                    i--;
                }
            }

            foreach (var e in elements)
            {
                if (!m_ShowedElements.Contains(e))
                {
                    e.Show(immediately);
                    m_ShowedElements.Add(e);
                }
            }
        }
    }
    
    public void Show<T>(bool immediately = false) where T : UIComposition
    {
        Show(typeof(T), immediately);
    }

    public T GetComposition<T>() where T : UIComposition
    {
        return (T)m_Compositions.Find(comp =>  comp.GetType() == typeof(T));
    }

    private void Start()
    {
        if (m_DefaultComposition) Show(m_DefaultComposition.GetType(), true);
    }
}
