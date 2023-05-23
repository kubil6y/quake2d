using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationTriggers : MonoBehaviour {
	[SerializeField] private Ghost _ghost;

	public void AnimationFinished() {
		_ghost.AnimationFinished();
	}
}
