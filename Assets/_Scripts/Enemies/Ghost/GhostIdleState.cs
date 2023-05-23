using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostIdleState : GhostState {
	public GhostIdleState(GhostStateMachine stateMachine, string stateName, Ghost ghost, int animBoolHash) : base(stateMachine, stateName, ghost, animBoolHash) {
	}
}
