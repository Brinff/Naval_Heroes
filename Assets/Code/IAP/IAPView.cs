using System;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Code.IAP
{
    public class IAPView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_CostLabel;
        [SerializeField]
        private CodelessIAPButton m_IAPButton;

        private void OnEnable()
        {
            m_IAPButton.onProductFetched.AddListener(OnProductFetched);
        }

        private void OnDisable()
        {
            m_IAPButton.onProductFetched.RemoveListener(OnProductFetched);
        }

        public void OnProductFetched(Product product)
        {
            if (product != null)
            {
                m_CostLabel.text = product.metadata.localizedPriceString;
            }
        }
    }
}