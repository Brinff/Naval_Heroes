using Code.Services;
using DG.Tweening;
using Extensions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
	[SerializeField] private float m_movingDuration = 0.5f;
	[SerializeField] private Ease m_movingEase = Ease.Unset;

	[Header("test points")]
	[SerializeField] private RectTransform m_spawnPoint;
	[SerializeField] private RectTransform m_destionationPoint;

	private void OnEnable()
	{
		ServiceLocator.Register(this);
	}

	private void OnDisable()
	{
		ServiceLocator.Unregister(this);
	}

	public async Task AnimateCoinsScreenSpace(RectTransform source, RectTransform destination, Action callback = null)
	{
		await AnimateCoinsScreenSpace(GetUIWorldPoint(source), GetUIWorldPoint(destination), m_coinPrefab, m_spawnCount, callback);
	}

	public async Task AnimateCoinsScreenSpace(RectTransform source, RectTransform destination, RectTransform prefab, int elementsCount, Action callback = null)
	{
		await AnimateCoinsScreenSpace(GetUIWorldPoint(source), GetUIWorldPoint(destination), prefab, elementsCount, callback);
	}

	public async Task AnimateCoinsScreenSpace(Vector2 spawnPoint, Vector2 destination, RectTransform prefab, int elementsCount, Action callback = null)
	{
		for (int i = 0; i < elementsCount; i++)
		{
			var randomOffset = new Vector2(UnityEngine.Random.Range(-m_randomSpawnPositionOffset.x, m_randomSpawnPositionOffset.x), UnityEngine.Random.Range(-m_randomSpawnPositionOffset.y, m_randomSpawnPositionOffset.y));
			var copy = Instantiate(prefab, m_spawningParent);
			copy.position = spawnPoint + randomOffset;
			copy.name = $"coin_{i}";
			if (i == elementsCount - 1)
			{
				await AnimateCoin(copy, destination, i, elementsCount).AsyncWaitForCompletion();
			}
			else
			{
				AnimateCoin(copy, destination, i, elementsCount);
			}
		}
	}

	public async Task AnimateMovement(RectTransform source, RectTransform destination, bool makeCopyOfSource = false, bool isScalingDown = false)
	{
		RectTransform movingObj;
		if (makeCopyOfSource)
		{
			var copy = Instantiate(source, m_spawningParent);
			copy.position = source.position;
			movingObj = copy;
		}
		else
		{
			movingObj = source;
		}
		await Task.Delay((int)(m_delayToMove * 1000));
		await MoveToDestination(movingObj, GetUIWorldPoint(destination), isScalingDown).AsyncWaitForCompletion();
	}

	[Button]
	public async void Test()
	{
		await AnimateCoinsScreenSpace(m_spawnPoint, m_destionationPoint);
	}

	private Tween AnimateCoin(RectTransform transform, Vector2 destination, int elementOrder, int elementCount, Action callback = null)
	{
		transform.gameObject.Disable();
		float randomSpawnDelay = Mathf.Lerp(m_minMaxSpawnDelay.x, m_minMaxSpawnDelay.y, ((float)elementOrder)/elementCount);

		Sequence sequence = DOTween.Sequence();

		return sequence.AppendInterval(randomSpawnDelay)
			.Append(transform.ScaleIn(m_scaleInEase, m_scaleInDuration))
			.AppendInterval(m_delayToMove)
			.Append(MoveToDestination(transform, destination).OnComplete(() =>
			{
				callback?.Invoke();
				Destroy(transform.gameObject);
			}));
	}

	private Tween MoveToDestination(RectTransform rectTransform, Vector2 destination, bool isScalingOut = false)
	{
		if (isScalingOut)
		{
			rectTransform.DOScale(0, m_movingDuration).SetEase(m_movingEase);
		}
		return rectTransform.DOMove(destination, m_movingDuration).SetEase(m_movingEase);
	}

	private Vector3 GetUIWorldPoint(RectTransform source)
	{
		var rectPos = source.rect.center;
		return source.TransformPoint(rectPos.x, rectPos.y, 0);
	}
}