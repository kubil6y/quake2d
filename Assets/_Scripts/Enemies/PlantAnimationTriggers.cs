using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAnimationTriggers : MonoBehaviour {
	[SerializeField] private Plant _plant;

	public void AnimationFinishedEvent() {
		_plant.AnimationFinishedEvent();
	}
}
