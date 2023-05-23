using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRangedWeapon : Weapon {
	public event EventHandler OnShoot;

	[SerializeField] private WeaponDataSO _weaponDataSO;
	[SerializeField] private Transform _muzzleTransform;
	[SerializeField] private ObjectPool _objectPool;

	private bool _isShooting;
	private int _damageMultiplier;
	private float _weaponDelayTimer;
	private float _weaponDelayDuration = 0.2f;
	private bool _isSingleFire;

	private void Start() {
		_weaponDelayDuration = _weaponDataSO.delayBetweenRounds;
		_isSingleFire = _weaponDataSO.isSingleFire;
	}

	private void Update() {
		_weaponDelayTimer -= Time.deltaTime;

		if (_isShooting && _weaponDelayTimer < 0) {
			_weaponDelayTimer = _weaponDelayDuration;

			SpawnProjectile();

			if (_isSingleFire) {
				_isShooting = false;
			}
		}
	}

	public void SpawnProjectile() {
		OnShoot?.Invoke(this, EventArgs.Empty);

		for (int i = 0; i < _weaponDataSO.bulletAmount; ++i) {
			GameObject projectileObject = _objectPool.GetPooledObject().gameObject;
			if (projectileObject == null) {
				return;
			}

			float spread = UnityEngine.Random.Range(-_weaponDataSO.spreadAngle, _weaponDataSO.spreadAngle);

			Quaternion bulletSpreadRotation = Quaternion.Euler(new Vector3(0, 0, spread));
			Quaternion rotationQuaternion = _muzzleTransform.rotation * bulletSpreadRotation;

			// align to gun barrel/muzzle position
			projectileObject.transform.SetPositionAndRotation(_muzzleTransform.position, rotationQuaternion);

			if (projectileObject.TryGetComponent(out Projectile projectile)) {
				bool isCriticalHit = UnityEngine.Random.value > (1 - _weaponDataSO.critChance);
				int damage = _weaponDataSO.damage;
				if (isCriticalHit) {
					damage = Mathf.RoundToInt(UnityEngine.Random.value * damage + damage);
				}
				projectile.Setup(damage * _damageMultiplier, _weaponDataSO.projectileSpeed, isCriticalHit);
			}
		}
	}

	public override void Shoot(int damageMultiplier) {
		_isShooting = true;
		_damageMultiplier = damageMultiplier;
	}

	public override void StopShooting() {
		_isShooting = false;
	}
}
