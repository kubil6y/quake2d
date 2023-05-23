using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffects : MonoBehaviour {
	[SerializeField] private Transform _healthPopupTransform;
	[SerializeField] private Health _health;
	[SerializeField] private ObjectPool _damagePopupPool;
	[SerializeField] private ObjectPool _bloodPool;
	[SerializeField] private ObjectPool _deathVFXPool;
	[SerializeField] private AudioClip _deathSound;
	[SerializeField] private AudioClip _hitmarkerSound;
	[SerializeField] private AudioClip _healSound;

	private float _lastTimeHitmarkerSoundPlayed;
	private float _hitmarkerSoundWindow = 0.35f;

	private void Start() {
		_health.OnHealed += Health_OnHealed;
		_health.OnDamaged += Health_OnDamaged;
		_health.OnDied += Health_OnDied;
	}

	public void Setup(
		ObjectPool damagePopupObjectPool,
		ObjectPool bloodObjectPool,
		ObjectPool deathVFXObjectPool) {
		_damagePopupPool = damagePopupObjectPool;
		_bloodPool = bloodObjectPool;
		_deathVFXPool = deathVFXObjectPool;
	}

	private void PlayHealSound() {
		AudioSource.PlayClipAtPoint(_healSound, transform.position, 1f);
	}

	private void PlayHitmarkerSound() {
		if (Time.time > _lastTimeHitmarkerSoundPlayed + _hitmarkerSoundWindow) {
			_lastTimeHitmarkerSoundPlayed = Time.time;
			AudioSource.PlayClipAtPoint(_hitmarkerSound, transform.position, 0.3f);
		}
	}

	private void Health_OnDied(object sender, Vector3 position) {
		AudioSource.PlayClipAtPoint(_deathSound, position, .4f);

		GameObject deathVFXObject = _deathVFXPool.GetPooledObject().gameObject;

		if (deathVFXObject == null) {
			return;
		}

		if (deathVFXObject.TryGetComponent<ParticleVFX>(out ParticleVFX deathVFX)) {
			deathVFX.SetupAndPlay(position);
		}
	}

	private void Health_OnHealed(object sender, int amount) {
		PlayHealSound();

		// damage popup
		GameObject healPopupObject = _damagePopupPool.GetPooledObject().gameObject;

		if (healPopupObject == null) {
			return;
		}

		if (healPopupObject.TryGetComponent<DamagePopup>(out DamagePopup healPopup)) {
			healPopup.SetupHeal(_healthPopupTransform.position, amount);
		}
	}

	private void Health_OnDamaged(object sender, Health.OnDamagedEventArgs e) {
		PlayHitmarkerSound();

		// damage popup
		GameObject damagePopupObject = _damagePopupPool.GetPooledObject().gameObject;

		if (damagePopupObject == null) {
			return;
		}

		if (damagePopupObject.TryGetComponent<DamagePopup>(out DamagePopup damagePopup)) {
			damagePopup.SetupDamage(_healthPopupTransform.position, e.amount, e.isCriticalHit);
		}

		// blood popup
		GameObject bloodObject = _bloodPool.GetPooledObject().gameObject;

		if (bloodObject == null) {
			return;
		}

		if (bloodObject.TryGetComponent<ParticleVFX>(out ParticleVFX particle)) {
			particle.SetupAndPlay(transform.position);
		}
	}

}