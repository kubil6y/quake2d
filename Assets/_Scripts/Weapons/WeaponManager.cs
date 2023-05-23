using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
	public event EventHandler OnWeaponSwap;

	[SerializeField] private Weapon[] _weaponList;
	[SerializeField] private Player _player;

	private Weapon _currentWeapon;

	private int _currentWeaponIndex = 1;
	private int _previousWeaponIndex;

	[SerializeField] private float _scrollCooldown = .2f;
	private float _scrollTimer;

	private void OnEnable() {
		_player.OnPlayerCanShoot += Player_OnPlayerCanShoot;
	}

	private void OnDisable() {
		_player.OnPlayerCanShoot -= Player_OnPlayerCanShoot;
	}

	private void Update() {
		HandleWeaponInput();
	}

	private void HandleWeaponInput() {
		_scrollTimer -= Time.deltaTime;

		if (Input.mouseScrollDelta.y > 0 && _scrollTimer < 0) {
			_scrollTimer = _scrollCooldown;
			int newWeaponIndex = _currentWeaponIndex - 1;
			if (newWeaponIndex < 0) {
				newWeaponIndex = _weaponList.Length-1;
			}
			SwapWeapon(newWeaponIndex);
		}
		if (Input.mouseScrollDelta.y < 0 && _scrollTimer < 0) {
			_scrollTimer = _scrollCooldown;
			int newWeaponIndex = (_currentWeaponIndex + 1) % _weaponList.Length;
			SwapWeapon(newWeaponIndex);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			SwapWeapon(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			SwapWeapon(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			SwapWeapon(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			SwapWeapon(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			SwapWeapon(4);
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			SwapWeapon(_previousWeaponIndex);
		}
	}

	private void Start() {
		foreach (Weapon weapon in _weaponList) {
			weapon.gameObject.SetActive(false);
		}

		if (_weaponList.Length > 0) {
			_currentWeapon = _weaponList[_currentWeaponIndex];
			_currentWeapon.gameObject.SetActive(true);
		}
	}

	public void Shoot() {
		_currentWeapon?.Shoot(_player.GetDamageMultiplier());
	}

	public void StopShooting() {
		_currentWeapon?.StopShooting();
	}

	// this should take new input!
	private void SwapWeapon(int newWeaponIndex) {
		if (newWeaponIndex == _currentWeaponIndex) {
			return;
		}

		if (newWeaponIndex < 0 || newWeaponIndex > _weaponList.Length) {
			Debug.LogError("index is shiet", this);
			return;
		}

		OnWeaponSwap?.Invoke(this, EventArgs.Empty);
		_previousWeaponIndex = _currentWeaponIndex;
		_currentWeaponIndex = newWeaponIndex;

		// swap
		_weaponList[_previousWeaponIndex].gameObject.SetActive(false);
		_currentWeapon = _weaponList[_currentWeaponIndex];
		_currentWeapon.gameObject.SetActive(true);

	}

	private void Player_OnPlayerCanShoot(object sender, bool canShoot) {
		if (canShoot) {
			_currentWeapon?.Show();
		} else {
			_currentWeapon?.Hide();
		}
	}
}
