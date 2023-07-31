using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
public class CrossWidget : MonoBehaviour, IUIElement
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
