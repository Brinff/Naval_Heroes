using Game.UI;
using System.Collections;
using UnityEngine;

namespace Code.IAP
{
    public class IAPShopWidget : MonoBehaviour, IUIElement
    {
        public void Hide(bool immediately)
        {
            gameObject.SetActive(false);
        }

        public void Show(bool immediately)
        {
            gameObject.SetActive(true);
        }
    }
}