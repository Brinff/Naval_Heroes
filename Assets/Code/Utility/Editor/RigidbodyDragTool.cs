using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEditor.PackageManager.UI;

[EditorTool("Rigidbody Drag Tool", typeof(Rigidbody))]
public class RigidbodyDragTool : EditorTool
{
    private int nameControll = "RigidbodyDragTool".GetHashCode();

    public override bool IsAvailable()
    {
        return Application.isPlaying;
    }

    private Rigidbody rigidbody;
    private Vector3 touchedRigidbodyPoint;
    private Vector3 targetRigidbodyPoint;
    private Plane plane;
    private RigidbodyDragToolOverlay toolOverlay;
    
    public static float forceMultiplier { get; set; }
    public static LayerMask layerMask { get; set; }

    private void LoadFields()
    {
        layerMask = EditorPrefs.GetInt("RDT_LM", 0);
        forceMultiplier = EditorPrefs.GetFloat("RDT_FM", 1);
    }

    private void SaveFields()
    {
        EditorPrefs.SetInt("RDT_LM", layerMask);
        EditorPrefs.SetFloat("RDT_FM", forceMultiplier);
    }

    private void OnEnable()
    {
        LoadFields();
        toolOverlay = new RigidbodyDragToolOverlay();
        SceneView.AddOverlayToActiveView(toolOverlay);
        toolOverlay.displayed = true;
    }

    private void OnDisable()
    {
        SaveFields();
        if (toolOverlay != null)
            SceneView.RemoveOverlayFromActiveView(toolOverlay);
        
    }

    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView sceneView))
            return;



        Event e = Event.current;



        int idControll = EditorGUIUtility.GetControlID(nameControll, FocusType.Passive);
        var type = e.GetTypeForControl(idControll);

        if (e.button == 0)
        {
            if (type == EventType.MouseDown)
            {
                var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.rigidbody != null)
                    {
                        rigidbody = hit.rigidbody;
                        touchedRigidbodyPoint = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                        plane = new Plane(-ray.direction, hit.point);
                        EditorGUIUtility.hotControl = idControll;
                        e.Use();
                    }
                }
            }

            if (type == EventType.Repaint)
            {
                if (EditorGUIUtility.hotControl == idControll)
                {
                    var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (plane.Raycast(ray, out float d) && rigidbody)
                    {
                        Vector3 point = ray.GetPoint(d);
                        Vector3 position = rigidbody.transform.TransformPoint(touchedRigidbodyPoint);
                        Vector3 force = point - position;
                        if (force != Vector3.zero)
                        {
                            Handles.ArrowHandleCap(idControll, position, Quaternion.LookRotation(force.normalized), force.magnitude, EventType.Repaint);
                            Handles.Label(position + force * 0.5f, $"{force.magnitude * forceMultiplier} F");
                        }
                        rigidbody.AddForceAtPosition(force * forceMultiplier, position, ForceMode.Acceleration);
                    }
                }


            }

            if (type == EventType.MouseUp && rigidbody)
            {
                rigidbody = null;
                EditorGUIUtility.hotControl = 0;
                e.Use();
            }
        }

        if (EditorGUIUtility.hotControl == idControll)
        {
            if (rigidbody == null)
            {
                EditorGUIUtility.hotControl = 0;
                return;
            }
        }
    }
}


