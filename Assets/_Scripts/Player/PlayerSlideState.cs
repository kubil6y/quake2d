using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerState {
	private float _slideTimer;
	public PlayerSlideState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		_slideTimer = player.GetSlideDuration();
		player.SetCollider(Player.Collider.Half);
		player.SetCanShoot(false);
	}

	public override void Exit() {
		base.Exit();
		player.SetCollider(Player.Collider.Full);
		player.SetCanShoot(true);
	}

	public override void Update() {
		_slideTimer -= Time.deltaTime;
		if (_slideTimer < 0) {
			stateMachine.TransitionTo(stateMachine.idleState);
		}
	}

	public override void FixedUpdate() {
		base.FixedUpdate();
		player.SetVelocity(player.GetFaceDir() * player.GetSlideSpeed(), player.rb.velocity.y);
	}
}