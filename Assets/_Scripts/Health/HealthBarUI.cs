using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
	[SerializeField] private Health _health;
	[SerializeField] private Image _image;

	protected virtual void Start() {
		_health.OnHealthChanged += Health_OnHealthChanged;
	}

	private void Health_OnHealthChanged(object sender, Health.OnHealthChangedEventArgs args) {
		_image.fillAmount = args.healthNormalized;
	}
}
