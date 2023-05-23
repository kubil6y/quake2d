using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState {
	private float _previousGravityScale;
	private float _dashTimer;

	public PlayerDashState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		_dashTimer = player.GetDashDuration();

		_previousGravityScale = player.rb.gravityScale;
		player.rb.gravityScale = 0;
	}

	public override void Exit() {
		base.Exit();
		player.rb.gravityScale = _previousGravityScale;
	}

	public override void Update() {
		base.Update();
		_dashTimer -= Time.deltaTime;
		if (_dashTimer < 0) {
			stateMachine.TransitionTo(stateMachine.idleState);
		}
	}

	public override void FixedUpdate() {
		base.FixedUpdate();
		float dashDirection;
		if (player.GetInputX() != 0) {
			dashDirection = player.GetInputX();
		} else {
			dashDirection = player.GetFaceDir();
		}
		player.SetVelocity(dashDirection * player.GetDashSpeed(), 0);
	}
}