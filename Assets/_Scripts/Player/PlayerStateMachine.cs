using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine {
	public PlayerState currentState { get; private set; }
	public Player player;

	#region states
	public PlayerIdleState idleState;
	public PlayerMoveState moveState;
	public PlayerJumpState jumpState;
	public PlayerFallState fallState;
	public PlayerDashState dashState;
	public PlayerCrouchState crouchState;
	public PlayerSlideState slideState;
	public PlayerWallSlideState wallSlideState;
	public PlayerWallJumpState wallJumpState;
	public PlayerDeathState deathState;
	#endregion // states

	#region animation keys
	private readonly int KEY_IDLE = Animator.StringToHash("Idle");
	private readonly int KEY_MOVE = Animator.StringToHash("Move");
	private readonly int KEY_JUMP = Animator.StringToHash("Jump");
	private readonly int KEY_DASH = Animator.StringToHash("Dash");
	private readonly int KEY_CROUCH = Animator.StringToHash("Crouch");
	private readonly int KEY_SLIDE = Animator.StringToHash("Slide");
	private readonly int KEY_WALL_SLIDE = Animator.StringToHash("WallSlide");
	private readonly int KEY_DEATH = Animator.StringToHash("Death");
	#endregion // animation keys

	public PlayerStateMachine(Player player) {
		this.player = player;

		idleState = new PlayerIdleState(this, player, KEY_IDLE);
		moveState = new PlayerMoveState(this, player, KEY_MOVE);
		jumpState = new PlayerJumpState(this, player, KEY_JUMP);
		fallState = new PlayerFallState(this, player, KEY_JUMP);
		dashState = new PlayerDashState(this, player, KEY_DASH);
		crouchState = new PlayerCrouchState(this, player, KEY_CROUCH);
		slideState = new PlayerSlideState(this, player, KEY_SLIDE);
		wallSlideState = new PlayerWallSlideState(this, player, KEY_WALL_SLIDE);
		wallJumpState = new PlayerWallJumpState(this, player, KEY_JUMP);
		deathState = new PlayerDeathState(this, player, KEY_DEATH);
	}

	private void Player_OnDied(object sender, EventArgs e) {
		TransitionTo(deathState);
	}

	public void TransitionTo(PlayerState newState) {
		currentState?.Exit();
		currentState = newState;
		newState.Enter();
	}
}