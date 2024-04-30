
using Code.Utility;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CustomEditor(typeof(ProgressionData))]
public class ProgressionDataEditor : Editor
{
    public override bool HasPreviewGUI()
    {
        return true;
    }

    private bool sampeSnap { get => EditorPrefs.GetBool("SAMPLE_SNAP", true); set => EditorPrefs.SetBool("SAMPLE_SNAP", value); }
    public override void OnPreviewSettings()
    {
        sampeSnap = EditorGUILayout.Toggle("Sample Snap", sampeSnap);
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        ProgressionData progressionData = target as ProgressionData;

        progressionData.maxStep = Mathf.Clamp(progressionData.maxStep, 10, 100000);

        Keyframe[] keys = new Keyframe[progressionData.maxStep];
        float maxValue = 0;
        for (int i = 0; i < progressionData.maxStep; i++)
        {
            var key = keys[i];

            key.value = progressionData.GetResult(i);
            key.time = i;
            maxValue = Mathf.Max(maxValue, key.value);
            keys[i] = key;
        }

        GUIStyle valueStyle = new GUIStyle("Label");
        valueStyle.alignment = TextAnchor.MiddleRight;

        GUI.BeginClip(r);
        {
            var area = new Rect(Vector2.zero, r.size);
            RectOffset paddingGraph = new RectOffset(60, 20, 20, 20);
            var rectGraph = paddingGraph.Remove(area);

            RectOffset paddingCurve = new RectOffset(10, 10, 10, 10);

            var curveArea = paddingCurve.Remove(rectGraph);

            Handles.DrawAAPolyLine(3, new Vector3[] { rectGraph.min, new Vector3(rectGraph.xMin, rectGraph.yMax), rectGraph.max });

            Vector3[] points = new Vector3[keys.Length];
            int everyStep = progressionData.maxStep / 10;
            for (int i = 0; i < keys.Length; i++)
            {
                points[i] = new Vector3(curveArea.x + curveArea.width * ((float)i / (keys.Length - 1)), curveArea.y + curveArea.height * (1 - (keys[i].value / maxValue)));

                if (i % everyStep == 0) Handles.Label(new Vector3(points[i].x, rectGraph.yMax + 10), i.ToString());
                if (i == keys.Length - 1) Handles.Label(new Vector3(points[i].x, rectGraph.yMax + 10), i.ToString());

                if (i == 0) Handles.Label(new Vector3(rectGraph.xMin - 10, points[i].y), ((int)keys[i].value).KiloFormat(), valueStyle);
                if (i == keys.Length - 1) Handles.Label(new Vector3(rectGraph.xMin - 10, points[i].y), ((int)keys[i].value).KiloFormat(), valueStyle);
            }

            var e = Event.current;
            int controll = EditorGUIUtility.GetControlID("Info".GetHashCode(), FocusType.Passive, curveArea);

            var type = e.GetTypeForControl(controll);

            if (type == EventType.MouseDown)
            {
                EditorGUIUtility.hotControl = controll;
                e.Use();
            }

            if (type == EventType.MouseDrag)
            {
                if (EditorGUIUtility.hotControl == controll)
                    e.Use();
            }

            if (type == EventType.MouseUp)
            {
                EditorGUIUtility.hotControl = 0;
                e.Use();
            }

            if (EditorGUIUtility.hotControl == controll)
            {
                float x = Mathf.Clamp(e.mousePosition.x, curveArea.min.x, curveArea.max.x);
                Color temp = Handles.color;
                Handles.color = Color.yellow;

                float position = MathfUtility.Remap(x, curveArea.min.x, curveArea.max.x, 0, progressionData.maxStep - 1);

                if (sampeSnap) position = Mathf.Round(position);


                float posX = curveArea.x + curveArea.width * (position / (progressionData.maxStep - 1));

                Handles.DrawAAPolyLine(2, new Vector3[] { new Vector3(posX, curveArea.yMin), new Vector3(posX, curveArea.yMax) });

                float value = progressionData.GetResult(position);
                float y = curveArea.y + curveArea.height * (1 - (value / maxValue));

                Handles.color = Color.red;
                Handles.CircleHandleCap(controll, new Vector3(posX, y), Quaternion.identity, 5, EventType.Repaint);
                Handles.Label(new Vector3(posX - 10, y - 10), $"[{position}|{value}]", valueStyle);
                Handles.color = temp;
            }


            Handles.DrawAAPolyLine(3, points);
        }
        GUI.EndClip();


    }
}
