using System;
using UnityEngine.UIElements;

namespace Code.Diagnostic
{
    public interface IDiagnostic
    {
        int order { get; }
        string path { get; }
        VisualElement CreateVisualTree();
    }
}