using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour {
	[SerializeField] private Player _player;
	[SerializeField] private SpriteRenderer _playerSpriteRenderer;
	[SerializeField] private Material _flashMaterial;
	[SerializeField] private Material _healMaterial;

	private Material _defaultMaterial;
	private Color _defaultColor;

	private void Start() {
		_defaultMaterial = _playerSpriteRenderer.material;
		_defaultColor = _playerSpriteRenderer.color;

		_player.health.OnHealed += Player_Health_OnHealed;
		_player.health.OnDamaged += Player_Health_OnDamaged;
		_player.OnQuadStarted += Player_OnQuadStarted;
		_player.OnQuadStopped += Player_OnQuadStopped;
	}

	private void Player_Health_OnHealed(object sender, int e) {
		StartCoroutine(HealEffectRoutine());
	}

	private void Player_Health_OnDamaged(object sender, Health.OnDamagedEventArgs e) {
		StartCoroutine(FlashEffectRoutine());
	}

	private IEnumerator HealEffectRoutine() {
		_playerSpriteRenderer.material = _healMaterial;
		yield return new WaitForSeconds(.15f);
		_playerSpriteRenderer.material = _defaultMaterial;
	}

	private IEnumerator FlashEffectRoutine() {
		_playerSpriteRenderer.material = _flashMaterial;
		yield return new WaitForSeconds(.1f);
		_playerSpriteRenderer.material = _defaultMaterial;

	}

	private void Player_OnQuadStarted(object sender, float e) {
		_playerSpriteRenderer.color = Color.cyan;
	}

	private void Player_OnQuadStopped(object sender, EventArgs e) {
		_playerSpriteRenderer.color = _defaultColor;
	}
}
