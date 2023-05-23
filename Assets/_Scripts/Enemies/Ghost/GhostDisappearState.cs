using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDisappearState : GhostState {
	public GhostDisappearState(GhostStateMachine stateMachine, string stateName, Ghost ghost, int animBoolHash) : base(stateMachine, stateName, ghost, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		ghost.circleCollider.enabled = false;
	}

	public override void Exit() {
		base.Exit();
		ghost.circleCollider.enabled = true;
	}

	public override void Update() {
		if (_animTriggerCalled) {
			stateMachine.TransitionTo(stateMachine.appearState);
		}
	}
}
