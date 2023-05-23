using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponEffects : MonoBehaviour {
	[SerializeField] private BaseRangedWeapon _weapon;
	[SerializeField] private WeaponDataSO _weaponDataSO;
	[SerializeField] private GameObject _muzzleFlash;
	[SerializeField] private Animator _animator;
	[SerializeField] private SpriteRenderer _spriteRenderer;

	protected float desiredAngle;

	private readonly int KEY_SHOOT = Animator.StringToHash("Shoot");

	private void OnEnable() {
		_weapon.OnShoot += Weapon_OnShoot;
	}

	private void OnDisable() {
		_weapon.OnShoot -= Weapon_OnShoot;
	}

	private void Update() {
		MouseFollow();
	}

	private void Weapon_OnShoot(object sender, EventArgs e) {
		_muzzleFlash.gameObject.SetActive(true);
		Invoke("HideMuzzleFlash", 0.05f);

		_animator.SetTrigger(KEY_SHOOT);

		AudioSource.PlayClipAtPoint(_weaponDataSO.shotFiredClip, transform.position);
	}

	private void HideMuzzleFlash() {
		_muzzleFlash.gameObject.SetActive(false);
	}

	private void MouseFollow() {
		var pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pointerPos.z = 0;
		var aimDirection = (Vector3)pointerPos - _weapon.transform.position;

		_weapon.transform.right = aimDirection;

		// flip weapon on y
		float desiredAngle =
			Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
		if (desiredAngle > 90 || desiredAngle < -90) {
			FlipWithDirection(-1);
		} else {
			FlipWithDirection(1);
		}
	}

	public void FlipWithDirection(int direction) {
		_weapon.transform.localScale = new Vector3(
			direction * Mathf.Abs(_weapon.transform.localScale.x),
			direction * Mathf.Abs(_weapon.transform.localScale.y),
			_weapon.transform.localScale.z
		);
	}
}
