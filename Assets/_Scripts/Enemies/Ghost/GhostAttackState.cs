using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttackState : GhostState {
	public GhostAttackState(GhostStateMachine stateMachine, string stateName, Ghost ghost, int animBoolHash) : base(stateMachine, stateName, ghost, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();

		ghost.ResetLifeTimer();
		SpawnProjectiles();
		stateMachine.TransitionTo(stateMachine.disappearState);
	}

	private void SpawnProjectiles() {
		float spreadAngle = ghost.GetSpreadAngle();
		float projectileAmount = ghost.GetProjectileAmount();

		float angleToPlayer = ghost.GetGhostToPlayerAngleInDegrees();
		float start = angleToPlayer - spreadAngle;
		float end = angleToPlayer + spreadAngle;
		float step = (end - start) / (projectileAmount - 1);

		List<float> angles = new List<float>();
		for (int i = 1; i <= projectileAmount; ++i) {
			angles.Add(start + step * i);
		}

		for (int i = 0; i < angles.Count; i++) {
			SpawnProjectile(angles[i]);
		}
	}

	private void SpawnProjectile(float angle) {
		Transform _projectileSpawnTransform = ghost.transform;

		GameObject projectileObject =
			ghost
				.GetProjectileObjectPool()
				.GetPooledObject().gameObject;

		projectileObject.transform.position = _projectileSpawnTransform.transform.position;
		Quaternion projectileRotation = Quaternion.Euler(new Vector3(0, 0, angle));
		projectileObject.transform.rotation = projectileRotation;

		if (projectileObject.TryGetComponent<Projectile>(out Projectile projectile)) {
			projectile.Setup(ghost.GetProjectileDamage(), ghost.GetProjectileMoveSpeed(), false, ghost.GetProjectileLifeTimerMax());
		}
	}
}
