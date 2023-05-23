using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class Projectile : MonoBehaviour {
	[SerializeField] private LayerMask _targetLayerMask;
	private int _damage;
	private bool _isCritcalHit;
	private float _moveSpeed;
	private float _lifeTimer;
	private float _lifeTimerMax = 5f;

	private PooledObject _pooledObject;

	private void Awake() {
		_pooledObject = GetComponent<PooledObject>();
	}

	public void Setup(int damage, float moveSpeed, bool isCriticalHit, float lifeTimerMax = 2f) {
		_damage = damage;
		_moveSpeed = moveSpeed;
		_isCritcalHit = isCriticalHit;
		_lifeTimerMax = lifeTimerMax;
	}

	private void FixedUpdate() {
		_lifeTimer += Time.deltaTime;
		if (_lifeTimer > _lifeTimerMax) {
			_lifeTimer = 0;
			_pooledObject.Release();
		}

		transform.position += (Vector3)(_moveSpeed * Time.fixedDeltaTime * transform.right);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// should require a layermask!
		// if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
		if (((1 << other.gameObject.layer) & _targetLayerMask) != 0) {
			if (other.TryGetComponent<IDamageable>(out IDamageable target)) {
				target.TakeDamage(_damage, _isCritcalHit);
				ReleaseSelf();
			}
		}
	}

	private void ReleaseSelf() {
		_lifeTimer = 0f;
		_pooledObject.Release();
	}
}
