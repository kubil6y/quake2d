using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState {
	public PlayerWallJumpState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();

		float xWallJumpVelocity = 5f;
		player.SetVelocity(xWallJumpVelocity * -player.GetFaceDir(), player.GetJumpForce());
	}

	public override void FixedUpdate() {
		base.FixedUpdate();

		if (player.IsGrounded()) {
			stateMachine.TransitionTo(stateMachine.idleState);
		}
	}
}
