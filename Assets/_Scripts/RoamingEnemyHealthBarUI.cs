using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingEnemyHealthBarUI : HealthBarUI {
	[SerializeField] RoamingEnemy _roamingEnemy;

	protected override void Start() {
		base.Start();
		_roamingEnemy.OnDirectionChanged += RoamingEnemy_OnDirectionChanged;
	}

	private void RoamingEnemy_OnDirectionChanged(object sender, int direction) {
		transform.localScale = new Vector3(
			direction * Mathf.Abs(transform.localScale.x),
			transform.localScale.y,
			transform.localScale.z
		);
	}
}
