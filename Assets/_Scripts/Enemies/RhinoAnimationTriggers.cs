using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoAnimationTriggers : MonoBehaviour {
	private Rhino _rhino;

	private void Awake() {
		_rhino = GetComponentInParent<Rhino>();
	}

	public void AnimationEnded() {
		// _rhino.AnimationEnded();
	}

	private void AttackTrigger() {
		Debug.Log("AttackTrigger()");

		/*
		// Temp collider array that will exist only for one frame!
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_Enemy.m_AttackCheck.position, m_Enemy.m_AttackCheckRadius);

		foreach (var hit in colliders) {
			if (hit.TryGetComponent<Player>(out Player player)) {
				PlayerStats targetStats = hit.GetComponent<PlayerStats>();
				m_Enemy.m_Stats.DoDamage(targetStats);
			}
		}
		*/
	}

}
