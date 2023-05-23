using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {
	[SerializeField] private List<Transform> _checkPointList;
	private int _currentIndex = 0;

	private void Start() {
		if (_checkPointList.Count == 0) {
			Debug.LogWarning("CheckpointList is empty");
		}
	}
}
