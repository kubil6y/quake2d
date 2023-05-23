using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarUI : HealthBarUI {
	[SerializeField] Player _player;

	protected override void Start() {
		base.Start();
		_player.OnDirectionChanged += Player_OnDirectionChanged;
	}

	private void Player_OnDirectionChanged(object sender, int direction) {
		transform.localScale = new Vector3(
			direction * Mathf.Abs(transform.localScale.x),
			transform.localScale.y,
			transform.localScale.z
		);
	}
}
