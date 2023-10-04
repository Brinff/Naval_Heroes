using Game.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameWidget : MonoBehaviour, IUIElement
{
    public event System.Action OnClick;
    [SerializeField]
    private TextMeshProUGUI m_LevelLabel;
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private CanvasGroup m_Group;
    public void SetBlock(bool isBlock)
    {
        m_Group.alpha = isBlock ? 0.5f : 1;
    }

    public Button GetButton()
    {
        return m_Button;
    }

    private void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }
    private void OnEnable()
    {
        m_Button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        OnClick?.Invoke();
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void SetLevel(int level)
    {
        m_LevelLabel.text = $"Mission {level}";
    }
}
