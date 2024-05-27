using TMPro;
using UnityEngine;

public class PopUpItem : MonoBehaviour
{
	[SerializeField] private RectTransform _rectTransform;
	[SerializeField] protected TextMeshProUGUI m_title;

	public RectTransform RectTransform => _rectTransform;
	public PopUpItemData PopUpItemData { get; private set; }

	public virtual void Initialise(PopUpItemData popUpItemData)
	{
		PopUpItemData = popUpItemData;

		SetTitle(popUpItemData.Title);
		gameObject.name += popUpItemData.Title;
	}

	public virtual void SetTitle(object title)
	{
		m_title.text = title.ToString();
	}
}
