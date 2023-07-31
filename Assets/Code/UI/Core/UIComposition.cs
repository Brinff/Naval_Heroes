using Game.UI;
using Sirenix.OdinInspector;


using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public abstract class UIComposition : MonoBehaviour
{
    private UICompositionModule m_CompositionModule;

    private void OnEnable()
    {
        m_CompositionModule = GetComponentInParent<UICompositionModule>();
        m_CompositionModule.Register(this);
    }

    private void OnDisable()
    {
        m_CompositionModule.Unregister(this);
    }

#if UNITY_EDITOR
    [Button]
    private void Show()
    {
        if (m_CompositionModule != null) m_CompositionModule.Show(this.GetType(), true);
    }
#endif



    [SerializeField, ListDrawerSettings(HideAddButton = true, OnTitleBarGUI = "DrawOnAdd")]
    private List<Element> m_Elements = new List<Element>();

    public IUIElement[] GetElements()
    {
        return m_Elements.Where(x => x.acitve).Select(x => x.element as IUIElement).ToArray();
    }

    [System.Serializable, HideLabel]
    public class Element
    {
        [HorizontalGroup("E"), HideLabel]
        public bool acitve;
        [HorizontalGroup("E"), LabelText("")]
        public MonoBehaviour element;
    }

#if UNITY_EDITOR
    private void DrawOnAdd()
    {
        if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
        {
            List<Object> objects = new List<Object>();
            var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            Scene scene = stage != null ? stage.scene : UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            var findElements = scene.GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<IUIElement>(true)).ToList();
            if (m_Elements != null)
            {
                GenericMenu genericMenu = new GenericMenu();
                for (int i = 0; i < findElements.Count; i++)
                {
                    var e = findElements[i] as MonoBehaviour;
                    if (m_Elements.Exists(x => x.element == findElements[i] as MonoBehaviour))
                    {
                        genericMenu.AddDisabledItem(new GUIContent(e.name));
                        continue;
                    }
                    genericMenu.AddItem(new GUIContent(e.name), false, () =>
                    {
                        m_Elements.Add(new Element() { element = e, acitve = true });
                        if (stage != null) UnityEditor.EditorUtility.SetDirty(stage.prefabContentsRoot);
                    });
                }
                genericMenu.ShowAsContext();
            }
        }
    }
#endif
}
