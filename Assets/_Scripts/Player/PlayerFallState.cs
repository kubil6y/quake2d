using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState {
	public PlayerFallState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Update() {
		base.Update();
	}

	public override void FixedUpdate() {
		base.FixedUpdate();

		if (player.IsGrounded()) {
			player.logger.Log("Fall->Idle");
			stateMachine.TransitionTo(stateMachine.idleState);
		}
	}
}
