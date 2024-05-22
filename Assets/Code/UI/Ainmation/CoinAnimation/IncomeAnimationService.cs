using Code.Services;
using DG.Tweening;
using Extensions;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class IncomeAnimationService : MonoBehaviour, IService
{
	[SerializeField] private RectTransform m_coinPrefab;
	[SerializeField] private Transform m_spawningParent;

	[Header("Animation settings")]
	[SerializeField] private int m_spawnCount;
	[SerializeField] private Vector2 m_minMaxSpawnDelay;
	[SerializeField] private Vector2 m_randomSpawnPositionOffset;
	[SerializeField] private float m_scaleInDuration = 0.5f;
	[SerializeField] private Ease m_scaleInEase = Ease.Unset;
	[SerializeField] private float m_delayToMove = 0.2f;
	[SerializeField] private float m_movingSpeed = 5;
	[SerializeField] private Ease m_movingEase = Ease.Unset;

	[Header("test points")]
	[SerializeField] private RectTransform m_spawnPoint;
	[SerializeField] private RectTransform m_destionationPoint;

	private Camera m_camera;

	private void Start()
	{
		m_camera = Camera.main;
	}

	private void OnEnable()
	{
		ServiceLocator.Register(this);
	}

	private void OnDisable()
	{
		ServiceLocator.Unregister(this);
	}

	public void AnimateScreenSpace(RectTransform source, RectTransform destination)
	{
		AnimateScreenSpace(GetUIWorldPoint(source), GetUIWorldPoint(destination), m_spawnCount);
	}

	public void AnimateScreenSpace(RectTransform source, RectTransform destination, int elementsCount)
	{
		AnimateScreenSpace(GetUIWorldPoint(source), GetUIWorldPoint(destination), elementsCount);
	}

	public void AnimateScreenSpace(Vector2 spawnPoint, Vector2 destination, int elementsCount)
	{
		for (int i = 0; i < elementsCount; i++)
		{
			var randomOffset = new Vector2(Random.Range(-m_randomSpawnPositionOffset.x, m_randomSpawnPositionOffset.x), Random.Range(-m_randomSpawnPositionOffset.y, m_randomSpawnPositionOffset.y));
			var copy = Instantiate(m_coinPrefab, m_spawningParent);
			copy.position = spawnPoint + randomOffset;
			copy.name = $"coin_{i}";
			StartCoroutine(Animate(copy, destination, i, elementsCount));
		}
	}

	[Button]
	public void Test()
	{
		AnimateScreenSpace(m_spawnPoint, m_destionationPoint);
	}

	private IEnumerator Animate(RectTransform transform, Vector2 destination, int elementOrder, int elementCount)
	{
		transform.gameObject.Disable();
		float randomSpawnDelay = Mathf.Lerp(m_minMaxSpawnDelay.x, m_minMaxSpawnDelay.y, ((float)elementOrder)/elementCount);

		yield return new WaitForSeconds(randomSpawnDelay);
		yield return transform.ScaleIn(m_scaleInEase, m_scaleInDuration);
		yield return new WaitForSeconds(m_delayToMove + m_scaleInDuration);
		yield return transform.DOMove(destination, m_movingSpeed).SetEase(m_movingEase).SetSpeedBased().OnComplete(() => Destroy(transform.gameObject));
	}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.blue;
	//	var pos = m_destionationPoint.rect.center;
	//	//var pos = m_destionationPoint.anchoredPosition;
	//	Gizmos.DrawSphere(m_destionationPoint.TransformPoint(pos.x, pos.y, 0), 20);
	//	Gizmos.color = Color.white;
	//	var pos2 = m_spawnPoint.rect.center;
	//	Gizmos.DrawSphere(m_spawnPoint.TransformPoint(pos2.x, pos2.y, 0), 20);
	//}

	private Vector3 GetUIWorldPoint(RectTransform source)
	{
		var rectPos = source.rect.center;
		return source.TransformPoint(rectPos.x, rectPos.y, 0);
	}
}