using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Diagnostic
{
    public class DiagnosticService : MonoBehaviour, IService, IInitializable, IDisposable
    {
        private static List<IDiagnostic> s_Collection = new List<IDiagnostic>();

        public static VisualElement CreateVisualTree()
        {
            TabbedViewDiagnostic tabbedView = new TabbedViewDiagnostic();
            foreach (var hierarchy in s_Hierarchy)
            {
                var scrollView = new ScrollView();
                scrollView.style.flexGrow = new StyleFloat(0f);
                scrollView.Add(hierarchy.CreateVisualTree());
                tabbedView.AddTab(new TabButtonDiagnostic(hierarchy.name, scrollView), false);
            }
            return tabbedView;
        }

        public static bool isInitalized { get; private set; }

        public delegate void InitializeDelegate();

        public delegate void DisposeDelegate();

        public static event InitializeDelegate OnInitialize;
        public static event DisposeDelegate OnDispose;

        [SerializeField]
        private bool m_IsShow;
        
        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
            Dispose();
        }

        [System.Serializable]
        public class HierarchyDiagnostic
        {
            public bool isRoot;
            public string name;
            public List<HierarchyDiagnostic> childs;
            public List<IDiagnostic> diagnostics;

            public void AddDiagnostic(IDiagnostic diagnostic)
            {
                if (diagnostics == null) diagnostics = new List<IDiagnostic>();
                diagnostics.Add(diagnostic);
            }
            
            public VisualElement CreateVisualTree()
            {
                
                
                var root = new VisualElement();

                if (childs != null && childs.Count > 0)
                {
                    TabbedViewDiagnostic tabbedView = new TabbedViewDiagnostic();
                    foreach (var child in childs)
                    {
                        tabbedView.AddTab(new TabButtonDiagnostic(child.name, child.CreateVisualTree()), false);
                    }
                    root.Add(tabbedView);
                }

                if (diagnostics != null && diagnostics.Count > 0)
                {
                    diagnostics = diagnostics.OrderBy(x => x.order).ToList();
                    foreach (var diagnostic in diagnostics)
                    {
                        root.Add(diagnostic.CreateVisualTree());
                    }
                }

                return root;
            }
        }

        private static List<HierarchyDiagnostic> s_Hierarchy = new List<HierarchyDiagnostic>();
        
        private static HierarchyDiagnostic FindOrCreate(string path)
        {
            string[] directories = path.Split('/');
            HierarchyDiagnostic hierarchy = null;
            List<HierarchyDiagnostic> childs = s_Hierarchy;
            foreach (var directory in directories)
            {
                if (hierarchy != null)
                {
                    if (hierarchy.childs == null) hierarchy.childs = new List<HierarchyDiagnostic>();
                    childs = hierarchy.childs;
                }
                hierarchy = childs.Find(x => x.name == directory);
                if (hierarchy == null)
                {
                    hierarchy = new HierarchyDiagnostic();
                    hierarchy.name = directory;
                    hierarchy.isRoot = s_Hierarchy == childs;
                    childs.Add(hierarchy);
                }
            }
            return hierarchy;
        }

        public static void Register(IDiagnostic diagnostic)
        {
            if (!s_Collection.Contains(diagnostic))
            {
                var hierarchy = FindOrCreate(diagnostic.path);
                hierarchy.AddDiagnostic(diagnostic);
                s_Collection.Add(diagnostic);
            }
        }

        public static void Unregister(IDiagnostic diagnostic)
        {
            if (s_Collection.Remove(diagnostic))
            {
            }
        }

        public void Initialize()
        {
            isInitalized = true;
            foreach (var diagnostic in s_Collection)
            {
                if(diagnostic is IInitializable initializable) initializable.Initialize();
            }
            OnInitialize?.Invoke();
            if (m_IsShow)
            {
                var element = CreateVisualTree();
                element.style.marginTop = new StyleLength(new Length(100, LengthUnit.Percent));
                GetComponent<UIDocument>().rootVisualElement.Add(element);
            }
        }

        public void Dispose()
        {
            s_Hierarchy.Clear();
            isInitalized = false;
            OnDispose?.Invoke();
        }
    }
}