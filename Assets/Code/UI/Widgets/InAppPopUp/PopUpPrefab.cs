using UnityEngine;

[System.Serializable]
public struct PopUpPrefab
{
	[SerializeField] private PopUpItemType m_popUpItemType;
	[SerializeField] private PopUpItem m_popUpItem;

	public PopUpItemType PopUpItemType => m_popUpItemType;
	public PopUpItem PopUpItem => m_popUpItem;
}
