using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState {
	private float _crouchTimer;

	public PlayerIdleState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		player.SetVelocityZero();
	}

	public override void Update() {
		base.Update();

		_crouchTimer -= Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.LeftControl) && _crouchTimer < 0) {
			_crouchTimer = player.GetCrouchCooldownDuration();
			stateMachine.TransitionTo(stateMachine.crouchState);
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			stateMachine.TransitionTo(stateMachine.jumpState);
		}

		if (player.GetInputX() != 0) {
			if (player.IsTouchingWall() && player.GetInputX() == player.GetFaceDir()) {
				return;
			}
			stateMachine.TransitionTo(stateMachine.moveState);
		}
	}

	public override void FixedUpdate() {
		base.FixedUpdate();

		if (!player.IsGrounded()) {
			stateMachine.TransitionTo(stateMachine.fallState);
		}
	}
}
