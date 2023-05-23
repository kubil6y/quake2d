using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState {
	public PlayerAirState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void FixedUpdate() {
		base.FixedUpdate();

		bool isGrounded = player.IsGrounded();
		bool isTouchingWall = player.IsTouchingWall();

		if (!isGrounded && isTouchingWall) {
			stateMachine.TransitionTo(stateMachine.wallSlideState);
		}

		if (player.GetInputX() != 0) {
			player.SetVelocity(player.GetOnAirXVelocityModifier() * player.GetInputX() * player.GetMovementSpeed(), player.rb.velocity.y);
		}
	}
}
