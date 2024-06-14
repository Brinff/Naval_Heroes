using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLayoutFix : MonoBehaviour
{
	[SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

	private bool _isInFix;

	private void OnEnable()
	{
		if (!_isInFix)
		{
			StartCoroutine(Fix());
		}
	}

	private IEnumerator Fix()
	{
		_isInFix = true;
		//var startSpacing = _verticalLayoutGroup.spacing;
		_verticalLayoutGroup.spacing++;
		yield return new WaitForEndOfFrame();
		_verticalLayoutGroup.spacing--;
		_isInFix = false;
	}
}
