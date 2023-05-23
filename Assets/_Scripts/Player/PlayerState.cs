using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState {
	protected PlayerStateMachine stateMachine;
	protected Player player;
	private int _animBoolHash;
	private float _dashCooldownTimer;

	protected PlayerState(PlayerStateMachine stateMachine, Player player, int animBoolHash) {
		this.stateMachine = stateMachine;
		this.player = player;
		_animBoolHash = animBoolHash;
	}

	public virtual void Enter() {
		player.animator.SetBool(_animBoolHash, true);
	}

	public virtual void Exit() {
		player.animator.SetBool(_animBoolHash, false);
	}

	public virtual void Update() { 
		_dashCooldownTimer -= Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.LeftShift) && _dashCooldownTimer < 0 && !player.IsTouchingWall()) {
			_dashCooldownTimer = player.GetDashCooldownDuration();
			stateMachine.TransitionTo(stateMachine.dashState);
		}

		if (Input.GetKeyDown(KeyCode.Mouse1) && !player.IsSlowMotion() && player.CanSlowMo()) {
			player.StartCoroutine(player.SlowMotionRoutine());
		}
	}

	public virtual void FixedUpdate() { }
}
