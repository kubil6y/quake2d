using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
	public abstract void Shoot(int damageMultiplier);
	public abstract void StopShooting();

	public virtual void Show() {
		gameObject.SetActive(true);
	}

	public virtual void Hide() {
		gameObject.SetActive(false);
	}
}