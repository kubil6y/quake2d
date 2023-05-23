using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {
	public event EventHandler<Vector3> OnDied;
	public event EventHandler<int> OnHealed;
	public class OnHealedEventArgs : EventArgs {
		public int amount;
	}
	public event EventHandler<OnDamagedEventArgs> OnDamaged;
	public class OnDamagedEventArgs : EventArgs {
		public int amount;
		public bool isCriticalHit;
	}

	public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
	public class OnHealthChangedEventArgs : EventArgs {
		public float healthNormalized;
	}

	// TODO: will be made private later on
	public int _currentHealth;
	public int _maxHealth;

	public void Setup(int maxHealth) {
		_maxHealth = maxHealth;
		_currentHealth = _maxHealth;
	}

	public bool IsAlive() {
		return _currentHealth > 0;
	}

	public void SetFullHealth() {
		if (_maxHealth <= 0) {
			Debug.LogWarning("max health is messed up!");
		}
		_currentHealth = _maxHealth;
		OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
			healthNormalized = GetHealthNormalized()
		});
	}

	public void Heal(int amount) {
		_currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);

		OnHealed?.Invoke(this, amount);
		OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
			healthNormalized = GetHealthNormalized(),
		});
	}

	public void TakeDamage(int damage, bool isCriticalHit) {
		if (!IsAlive()) {
			return;
		}
		_currentHealth -= damage;

		OnDamaged?.Invoke(this, new OnDamagedEventArgs {
			amount = damage,
			isCriticalHit = isCriticalHit,
		});

		OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
			healthNormalized = (float)_currentHealth / _maxHealth,
		});

		_currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);


		if (_currentHealth == 0) {
			OnDied?.Invoke(this, transform.position);
		}
	}

	public int GetCurrentHealth() {
		return _currentHealth;
	}

	private float GetHealthNormalized() {
		return (float)_currentHealth / _maxHealth;
	}
}
