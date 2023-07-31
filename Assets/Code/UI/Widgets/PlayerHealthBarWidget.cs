using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private HealthBar m_HealthBar;
    public HealthBar healthBar => m_HealthBar;

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }
}
