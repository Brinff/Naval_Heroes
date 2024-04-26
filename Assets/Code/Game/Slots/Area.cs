using UnityEngine;

namespace Code.Game.Slots
{
    public class Area : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_RectTransform;
        
        public bool Overlap(Vector3 position, Rect rect, out float weight)
        {
            weight = 0f;
            var itemHandleRect = new Rect(0, 0, 10, 10);
            Vector3 localPosition = m_RectTransform.InverseTransformPoint(position);
            rect.center += (Vector2)localPosition;
            
            if (m_RectTransform.rect.Overlaps(rect))
            {
                var intersectRect = m_RectTransform.rect.GetIntersectRect(rect);
                weight = intersectRect.GetArea() / rect.GetArea();
                return true;
            }
            return false;
        }
    }
}