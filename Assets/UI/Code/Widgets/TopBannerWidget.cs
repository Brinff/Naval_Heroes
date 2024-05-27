using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;

namespace Code.UI.Widgets
{
    public class TopBannerLayoutWidget : MonoBehaviour, IUIElement
    {
        [SerializeField]
        private RectTransform m_Layout;
        
        public void Show(bool immediately)
        {
            gameObject.SetActive(true);
        }

        public void Hide(bool immediately)
        {
            gameObject.SetActive(false);
        }
        
        public void AddBannerView(Component product)
        {
            product.transform.SetParent(m_Layout, false);
        }
    }
}
