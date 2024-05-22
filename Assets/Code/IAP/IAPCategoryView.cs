using UnityEngine;

namespace Code.IAP
{
    public class IAPCategoryView : MonoBehaviour
    {
        [SerializeField]
        private IAPCategory m_Category;
        public IAPCategory category => m_Category;
        [SerializeField]
        private RectTransform m_Layout;
        
        public void AddProductView(Component product)
        {
            product.transform.SetParent(m_Layout);
        }
    }
}