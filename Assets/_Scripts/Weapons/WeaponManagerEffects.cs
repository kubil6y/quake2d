using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagerEffects : MonoBehaviour {
	[SerializeField] private AudioClip _weaponSwapClip;
	[SerializeField] private WeaponManager _weaponManager;

	private void OnEnable() {
		_weaponManager.OnWeaponSwap += WeaponManager_OnWeaponSwap;
	}

	private void OnDisable() {
		_weaponManager.OnWeaponSwap -= WeaponManager_OnWeaponSwap;
	}

	private void WeaponManager_OnWeaponSwap(object sender, EventArgs e) {
		AudioSource.PlayClipAtPoint(_weaponSwapClip, transform.position);
	}
}
