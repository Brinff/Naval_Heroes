using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpItem : MonoBehaviour
{
	[SerializeField] private Image m_preview;
	[SerializeField] private RectTransform _rectTransform;
	[SerializeField] protected TextMeshProUGUI m_title;

	public RectTransform RectTransform => _rectTransform;
	public PopUpItemData PopUpItemData { get; private set; }

	public virtual void Initialise(PopUpItemData popUpItemData)
	{
		PopUpItemData = popUpItemData;

		SetPreview(popUpItemData.Sprite);
		SetTitle(popUpItemData.Title);
		gameObject.name += popUpItemData.Title;
	}

	public virtual void SetPreview(Sprite sprite)
	{
		m_preview.sprite = sprite;
		//m_preview.SetNativeSize();
	}

	public virtual void SetTitle(object title)
	{
		m_title.text = title.ToString();
	}
}
