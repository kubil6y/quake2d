using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponEffects : MonoBehaviour {
	[SerializeField] private MeleeWeapon _weapon;
	[SerializeField] private AudioSource _audio;

	private void OnEnable() {
		_weapon.OnAttack += MeleeWeapon_OnAttack;
		_weapon.OnAttackStopped += MeleeWeapon_OnAttackStopped;
	}

	private void OnDisable() {
		_weapon.OnAttack -= MeleeWeapon_OnAttack;
		_weapon.OnAttackStopped -= MeleeWeapon_OnAttackStopped;
	}

	private void Update() {
		MouseFollow();
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

	private void MeleeWeapon_OnAttack(object sender, EventArgs e) {
		_audio.Play();
	}

	private void MeleeWeapon_OnAttackStopped(object sender, EventArgs e) {
		_audio.Stop();
	}
}
