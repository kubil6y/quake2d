using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostState {
	protected GhostStateMachine stateMachine;
	protected Ghost ghost;
	private int _animBoolHash;
	protected bool _animTriggerCalled;
	public string stateName;

	public GhostState(GhostStateMachine stateMachine, string stateName, Ghost ghost, int animBoolHash) {
		this.stateMachine = stateMachine;
		this.ghost = ghost;

		this.stateName = stateName;
		_animBoolHash = animBoolHash;
	}

	public virtual void Enter() {
		ghost.animator.SetBool(_animBoolHash, true);
		_animTriggerCalled = false;
	}

	public virtual void Exit() {
		ghost.animator.SetBool(_animBoolHash, false);
	}

	public virtual void Update() {
	}

	public virtual void FixedUpdate() {
	}

	public void AnimationFinished() {
		_animTriggerCalled = true;
	}
}