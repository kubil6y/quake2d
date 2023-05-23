using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState {
	public PlayerDeathState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		player.SetCanShoot(false);
	}

	public override void Exit() {
		base.Exit();
		player.SetCanShoot(true);
	}

	public override void FixedUpdate() {
		base.FixedUpdate();
		if (player.IsGrounded()) {
			player.SetVelocityZero();
		}
	}
}