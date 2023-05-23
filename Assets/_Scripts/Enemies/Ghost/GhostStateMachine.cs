using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateMachine {
	public Ghost ghost;
	public Transform playerTransform;

	public GhostIdleState idleState;
	public GhostAppearState appearState;
	public GhostDisappearState disappearState;
	public GhostDeathState deathState;
	public GhostAttackState attackState;
	public GhostMoveState moveState;

	private readonly int KEY_FLY = Animator.StringToHash("Fly");
	private readonly int KEY_APPEAR = Animator.StringToHash("Appear");
	private readonly int KEY_DISAPPEAR = Animator.StringToHash("Disappear");

	private GhostState _currentState;

	public GhostStateMachine(Ghost ghost, Transform playerTransform) {
		this.ghost = ghost;
		this.playerTransform = playerTransform;

		idleState = new GhostIdleState(this, "idle", ghost, KEY_FLY);
		appearState = new GhostAppearState(this, "appear", ghost, KEY_APPEAR);
		disappearState = new GhostDisappearState(this, "disappear", ghost, KEY_DISAPPEAR);
		moveState = new GhostMoveState(this, "move", ghost, KEY_FLY);
		attackState = new GhostAttackState(this, "attack", ghost, KEY_FLY);
		deathState = new GhostDeathState(this, "death", ghost, KEY_FLY);
	}

	public void TransitionTo(GhostState newState) {
		_currentState?.Exit();
		_currentState = newState;
		newState.Enter();
	}

	public void Update() {
		_currentState?.Update();
	}

	public void FixedUpdate() {
		_currentState?.FixedUpdate();
	}

	public void AnimationFinished() {
		_currentState.AnimationFinished();
	}
}
