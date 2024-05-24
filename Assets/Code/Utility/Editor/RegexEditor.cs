using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Code.Utility.Editor
{
    public class RegexEditor : EditorWindow
    {
        [MenuItem("Tools/Regex")]
        private static void ShowWindow()
        {
            var window = GetWindow<RegexEditor>();
            window.titleContent = new GUIContent("Regex");
            window.Show();
        }

        private string m_Input;
        private string m_Patern;
        private string m_Result;
        private void OnGUI()
        {
            m_Input = EditorGUILayout.TextArea(m_Input);
            m_Patern = EditorGUILayout.TextArea(m_Patern);
            try
            {
                var regex = new Regex(m_Patern);
                var ma = regex.Match(m_Input);
                m_Result = ma.Value;
            }
            catch (Exception e)
            {
                m_Result = e.Message;
            }

            EditorGUILayout.TextArea(m_Result);
        }
    }
}