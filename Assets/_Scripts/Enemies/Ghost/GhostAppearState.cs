using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAppearState : GhostState {
	public GhostAppearState(GhostStateMachine stateMachine, string stateName, Ghost ghost, int animBoolHash) : base(stateMachine, stateName, ghost, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
	}

	public override void Update() {
		base.Update();

		if (_animTriggerCalled) {
			stateMachine.TransitionTo(stateMachine.moveState);
		}
	}
}
