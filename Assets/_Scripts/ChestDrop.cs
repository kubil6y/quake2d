using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDrop : MonoBehaviour {
	[SerializeField] private Chest _chest;

	[SerializeField] private GameObject _dropPrefab;
	[SerializeField] private Transform _spawnTransform;

	private void Start() {
		_chest.OnInteracted += Chest_OnInteracted;
	}

	private void Chest_OnInteracted(object sender, EventArgs e) {
		Instantiate(_dropPrefab, _spawnTransform);
	}
}
