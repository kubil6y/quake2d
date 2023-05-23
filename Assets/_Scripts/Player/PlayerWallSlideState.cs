using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState {
	public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		player.SetCanShoot(false);
	}

	public override void Exit() {
		base.Exit();
		player.SetCanShoot(true);
	}

	public override void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			stateMachine.TransitionTo(stateMachine.wallJumpState);
		}
	}

	public override void FixedUpdate() {
		base.Update();

		// player can leave from wallslide
		if (player.GetInputX() != 0 && player.GetFaceDir() != player.GetInputX()) {
			stateMachine.TransitionTo(stateMachine.idleState);
		}

		bool isGrounded = player.IsGrounded();
		bool isTouchingWall = player.IsTouchingWall();

		if (isGrounded) {
			stateMachine.TransitionTo(stateMachine.idleState);
		}

		if (!isGrounded && !isTouchingWall) {
			stateMachine.TransitionTo(stateMachine.fallState);
		}

		float yVelModifier = player.GetInputY() < 0 ? 1 : 0.7f;
		player.SetVelocity(0, player.rb.velocity.y * yVelModifier);
	}

}