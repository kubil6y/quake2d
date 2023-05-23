using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerState {
	public PlayerCrouchState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		player.SetCollider(Player.Collider.Half);
		player.SetCanShoot(false);
	}

	public override void Exit() {
		base.Exit();
		player.SetCollider(Player.Collider.Full);
		player.SetCanShoot(true);
	}

	public override void Update() {
		base.Update();

		if (Input.GetKeyUp(KeyCode.LeftControl)) {
			stateMachine.TransitionTo(stateMachine.idleState);
		}
	}
}
