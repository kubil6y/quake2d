using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour {
	private PooledObject _pooledObject;

	private const float DISAPPEAR_TIMER_MAX = 0.5f;
	private TextMeshPro _textMesh;
	private Color _textColor;
	private float _disappearTimer;
	private Vector3 _moveVector;
	private static int _sortingOrder;

	// offset will be applied if popups happen one after another
	// and offset will be reset if it goes over 200ms
	private static float _offsetDuration = 0.2f;
	private static int _offsetCounter;
	private static float _lastTimeSpawned;

	private void Awake() {
		_textMesh = GetComponent<TextMeshPro>();
		_pooledObject = GetComponent<PooledObject>();
	}

	public void SetupHeal(Vector3 position, int amount) {
		transform.position = position;
		transform.localScale = Vector3.one;

		_textMesh.text = "+" + amount.ToString();

		_textMesh.fontSize = 7f;
		// TODO choose a better green later
		ColorUtility.TryParseHtmlString("#b8bb26", out _textColor);

		_moveVector = new Vector3(1, 1) * 2f;

		_textMesh.color = _textColor;
		_disappearTimer = DISAPPEAR_TIMER_MAX;

		_sortingOrder++;
		_textMesh.sortingOrder = _sortingOrder;
	}

	public void SetupDamage(Vector3 position, int damageAmount, bool isCriticalHit) {
		Vector3 offset;
		if (Time.time > _lastTimeSpawned + _offsetDuration) {
			_offsetCounter = 0;
			offset = Vector3.zero;
		} else {
			_offsetCounter++;
			Vector2 offsetAmount = new Vector2(0.05f, 0.05f);
			offset = _offsetCounter * offsetAmount;
		}
		_lastTimeSpawned = Time.time;

		transform.position = position + offset;
		transform.localScale = Vector3.one;

		_textMesh.text = damageAmount.ToString();
		if (!isCriticalHit) {
			// Normal hit
			_textMesh.fontSize = 5f;
			ColorUtility.TryParseHtmlString("#FFC500", out _textColor);
		} else {
			// Critical hit
			_textMesh.fontSize = 7f;
			ColorUtility.TryParseHtmlString("#D92C2C", out _textColor);
		}

		_moveVector = new Vector3(1, 1) * 2f;

		_textMesh.color = _textColor;
		_disappearTimer = DISAPPEAR_TIMER_MAX;

		_sortingOrder++;
		_textMesh.sortingOrder = _sortingOrder;
	}

	private void Update() {
		transform.position += _moveVector * Time.deltaTime;
		_moveVector -= _moveVector * 8f * Time.deltaTime;

		_disappearTimer -= Time.deltaTime;

		if (_disappearTimer > DISAPPEAR_TIMER_MAX * .5f) {
			// first half of the popup lifetime
			float increaseScaleAmount = 1f;
			transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
		} else {
			// second half of the popup lifetime
			float decreaseScaleAmount = 1f;
			transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
		}

		if (_disappearTimer < 0) {
			float disappearSpeed = 3f;
			_textColor.a -= disappearSpeed * Time.deltaTime;
			_textMesh.color = _textColor;

			if (_textColor.a < 0) {
				_pooledObject.Release();
			}
		}
	}
}
