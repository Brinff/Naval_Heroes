using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpItem : MonoBehaviour
{
	[SerializeField] private Image m_preview;
	[SerializeField] private TextMeshProUGUI m_title;

	public void Initialise(PopUpItemData popUpItemData)
	{
		m_preview.sprite = popUpItemData.Sprite;
		m_title.SetText(popUpItemData.Title);
		gameObject.name += popUpItemData.Title;
		m_preview.SetNativeSize();
	}
}
