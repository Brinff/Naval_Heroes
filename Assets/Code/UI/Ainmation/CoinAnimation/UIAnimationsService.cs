using Code.Services;
using DG.Tweening;
using UnityEngine;

public class UIAnimationsService : MonoBehaviour, IService
{
	[SerializeField] private Transform m_spawningParent;

	[Header("Animation settings")]
	[SerializeField] private float m_movingDuration = 0.5f;
	[SerializeField] private Ease m_movingEase = Ease.Unset;

	private void OnEnable()
	{
		ServiceLocator.Register(this);
	}

	private void OnDisable()
	{
		ServiceLocator.Unregister(this);
	}

	public Tween Animate(RectTransform source, RectTransform destination, bool isScalingOut = false)
	{
		var copy = Instantiate(source, m_spawningParent);
		var destinationPosition = GetUIWorldPoint(destination);

		if (isScalingOut)
		{
			copy.DOScale(0, m_movingDuration);
		}

		return copy.DOMove(destinationPosition, m_movingDuration).SetEase(m_movingEase);
	}

	private Vector3 GetUIWorldPoint(RectTransform source)
	{
		var rectPos = source.rect.center;
		return source.TransformPoint(rectPos.x, rectPos.y, 0);
	}
}