using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDeathState : GhostState {
	public GhostDeathState(GhostStateMachine stateMachine, string stateName, Ghost ghost, int animBoolHash) : base(stateMachine, stateName, ghost, animBoolHash) {
	}

	public override void Enter() {
		base.Enter();
		ghost.DestroySelf();
	}

	public override void Exit() {
		base.Exit();
	}
}
