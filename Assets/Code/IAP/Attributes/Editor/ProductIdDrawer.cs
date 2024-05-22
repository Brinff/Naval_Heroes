using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Code.IAP.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(ProductIdAttribute))]
    public class ProductIdDrawer : PropertyDrawer
    {
        private const string k_NoProduct = "<None>";
        private List<string> m_ValidIDs = new List<string>();
        private void LoadProductIdsFromCodelessCatalog()
        {
            var catalog = ProductCatalog.LoadDefaultCatalog();

            m_ValidIDs.Clear();
            m_ValidIDs.Add(k_NoProduct);
            foreach (var product in catalog.allProducts)
            {
                m_ValidIDs.Add(product.id);
            }
        }

        private string GetCurrentlySelectedProduct(GUIContent label, string productId)
        {
            var currentIndex = string.IsNullOrEmpty(productId) ? 0 : m_ValidIDs.IndexOf(productId);
            var newIndex = EditorGUILayout.Popup(label, currentIndex, m_ValidIDs.ToArray());
            return newIndex > 0 && newIndex < m_ValidIDs.Count ? m_ValidIDs[newIndex] : string.Empty;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LoadProductIdsFromCodelessCatalog();
            property.stringValue = GetCurrentlySelectedProduct(label, property.stringValue);
        }
    }
}