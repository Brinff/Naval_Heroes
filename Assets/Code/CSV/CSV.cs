using System;
using System.Text;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.CSV
{
    [System.Serializable]
    public class CSVTable
    {
        [SerializeField] private bool m_Enable;
        [SerializeField] private string m_Name;

        [FolderPath(AbsolutePath = true), SerializeField]
        private string m_Folder;

        [ShowInInspector] public string path => Path.Join(m_Folder, $"{m_Name}.csv");

        public CSVTable()
        {
        }

        public CSVTable(string folder)
        {
            m_Folder = folder;
        }

        public bool isExists => File.Exists(path);

        [Button]
        public void Clear()
        {
            if (isExists)
            {
                File.Delete(path);
            }
        }

        public void AddLine(params object[] values)
        {
            if(!m_Enable)return;
            string text = String.Join(';', values);
            if (!isExists)
            {
                Directory.CreateDirectory(m_Folder);
            }

            File.AppendAllText(path, text + Environment.NewLine);
        }

        [Button]
        public void AddLine(params string[] values)
        {
            if(!m_Enable)return;
            string text = String.Join(';', values);
            if (!isExists)
            {
                Directory.CreateDirectory(m_Folder);
            }

            File.AppendAllText(path, text + Environment.NewLine);
        }
    }
}