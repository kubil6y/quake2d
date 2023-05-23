using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MeleeWeapon : Weapon {
	public event EventHandler OnAttack;
	public event EventHandler OnAttackStopped;

	[SerializeField] private LayerMask _targetLayerMask;
	[SerializeField] private int _damage;
	[SerializeField] private Transform _attackCenterTransform;
	[SerializeField] private float _attackRadius;
	[SerializeField] private float _attackDelayDuration;
	private float _attackTimer;
	private bool _isAttacking;
	private int _damageMultiplier;

	private void Update() {
		_attackTimer -= Time.deltaTime;
		if (_isAttacking && _attackTimer < 0) {
			_attackTimer = _attackDelayDuration;
			Attack();
		}
	}

	private void Attack() {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCenterTransform.position, _attackRadius, _targetLayerMask);
		foreach(var collider in colliders) {
			if (collider.TryGetComponent<IDamageable>(out IDamageable target )) { 
				target.TakeDamage(_damage * _damageMultiplier, true);
			 }
		}
	}

	public override void Shoot(int damageMultiplier) {
		_isAttacking = true;
		_damageMultiplier = damageMultiplier;
		OnAttack?.Invoke(this, EventArgs.Empty);
	}

	public override void StopShooting() {
		_isAttacking = false;
		OnAttackStopped?.Invoke(this, EventArgs.Empty);
	}

	private void OnDrawGizmos() {
		Gizmos.DrawWireSphere(_attackCenterTransform.position, _attackRadius);
	}
}
