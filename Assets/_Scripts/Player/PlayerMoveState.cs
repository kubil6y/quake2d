using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState {
	private float _previousGravityScale;
	private float _coyoteTimer;
	private float _lastGroundedTime;
	private bool _isCoyote;

	private float _slideCooldownTimer;

	public PlayerMoveState(PlayerStateMachine stateMachine, Player player, int animBoolHash) : base(stateMachine, player, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		_previousGravityScale = player.rb.gravityScale;

		_lastGroundedTime = 0;
		_coyoteTimer = player.GetCoyoteTimerDuration();
		_isCoyote = false;
	}

	public override void Exit() {
		base.Exit();
		player.rb.gravityScale = _previousGravityScale;
	}

	public override void Update() {
		base.Update();

		if (player.GetInputX() == 0) {
			stateMachine.TransitionTo(stateMachine.idleState);
		}

		_slideCooldownTimer -= Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.LeftControl) && _slideCooldownTimer < 0) {
			_slideCooldownTimer = player.GetSlideCooldownDuration();
			stateMachine.TransitionTo(stateMachine.slideState);
		}

		if (CanJump()) {
			stateMachine.TransitionTo(stateMachine.jumpState);
		}
	}

	public override void FixedUpdate() {
		base.FixedUpdate();

		HandleCoyoteState();

		if (_isCoyote) {
			player.rb.gravityScale = 0;
		}

		if (CanFall()) {
			stateMachine.TransitionTo(stateMachine.fallState);
		}

		if (player.IsTouchingWall()) {
			stateMachine.TransitionTo(stateMachine.idleState);
		}

		player.SetVelocity(player.GetMovementSpeed() * player.GetInputX(), player.rb.velocity.y);
	}


	private bool CanJump() {
		bool jumpkeyPressed = Input.GetKeyDown(KeyCode.Space);
		return (jumpkeyPressed && player.IsGrounded()) || (jumpkeyPressed && _isCoyote);
	}

	private bool CanFall() {
		return !player.IsGrounded() && !_isCoyote;
	}

	private void HandleCoyoteState() {
		if (player.IsGrounded()) {
			_lastGroundedTime = Time.time;
		}

		if (!player.IsGrounded() && _lastGroundedTime != 0) {
			_coyoteTimer -= Time.deltaTime;
			_isCoyote = _coyoteTimer > 0;
		}
	}
}
