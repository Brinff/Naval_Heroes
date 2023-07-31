using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


//[Overlay(typeof(SceneView), "RigidbodyDragTool")]
public class RigidbodyDragToolOverlay : Overlay
{
    protected override Layout supportedLayouts => Layout.Panel;
    public override VisualElement CreatePanelContent()
    {
        VisualElement visualElement = new VisualElement() { name = "RigidbodyDragToolOverlay" };
        visualElement.Add(new Label("Rigidbody Drag"));

        var layerMaskField = new LayerMaskField("Raycast Mask", 0) { value = RigidbodyDragTool.layerMask };
        layerMaskField.RegisterValueChangedCallback(x => RigidbodyDragTool.layerMask = x.newValue);
        visualElement.Add(layerMaskField);

        var forceMultiplierField = new FloatField("Force Multiplier") { value = RigidbodyDragTool.forceMultiplier };
        forceMultiplierField.RegisterValueChangedCallback(x => RigidbodyDragTool.forceMultiplier = x.newValue);
        visualElement.Add(forceMultiplierField);

        return visualElement;
    }
}
