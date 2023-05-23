using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMoveState : GhostState {
	// sin movement
	private Vector3 _axis;
	private Vector3 _pos;

	private float _attackTimer;

	public GhostMoveState(GhostStateMachine stateMachine, string stateName, Ghost ghost, int animBoolHash) : base(stateMachine, stateName, ghost, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();

		_pos = ghost.transform.position;
		_axis = ghost.transform.up;
	}

	public override void Update() {
		base.Update();

		_attackTimer -= Time.deltaTime;

		if (CanAttack()) {
			_attackTimer = ghost.GetAttackCooldown();
			stateMachine.TransitionTo(stateMachine.attackState);
		}

		_pos += ghost.GetFacingDirectionValue() * ghost.transform.right * Time.deltaTime * ghost.GetMoveSpeed();
		ghost.transform.position = _pos + _axis * Mathf.Sin(Time.time * ghost.GetFrequency()) * ghost.GetMagnitude();
	}

	private bool CanAttack() {
		return ghost.GetDistanceToPlayer() < ghost.GetAttackDistance() && _attackTimer < 0;
	}
}
