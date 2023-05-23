using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimationTriggers : MonoBehaviour {
	[SerializeField] private Chest _chest;

	private void AnimationFinished() {
		_chest.InvokeAnimationFinished();
	}
}
