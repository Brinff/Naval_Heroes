using UnityEngine;

namespace Game.UI.Grid
{
    [System.Serializable]
    public class GridRect
    {
        [SerializeField]
        private Vector2Int m_Position;
        [SerializeField]
        private Vector2Int m_Size;
        
        public bool isDirty { get; set; }

        public Vector2Int position
        {
            get => m_Position;
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    isDirty = true;
                }
            }
        }

        public Vector2Int size
        {
            get => m_Size;
            set
            {
                if (m_Size != value)
                {
                    m_Size = value;
                    isDirty = true;
                }
            }
        }
    }
}