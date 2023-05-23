using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState {
	private bool _wallJumped;
	public PlayerJumpState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		_wallJumped = false;
		player.SetVelocity(new Vector2(player.rb.velocity.x, player.GetJumpForce()));
	}

	public override void Update() {
		base.Update();

		if (player.IsTouchingWall() && !_wallJumped && Input.GetKeyDown(KeyCode.Space)) {
			_wallJumped = true;
			player.SetVelocity(new Vector2(player.rb.velocity.x, player.GetJumpForce()));
		}
	}

	public override void FixedUpdate() {
		base.FixedUpdate();

		if (player.rb.velocity.y < 0) {
			stateMachine.TransitionTo(stateMachine.fallState);
		}
	}
}
