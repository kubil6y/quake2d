using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
	[SerializeField] private LayerMask _targetLayerMask;
	[SerializeField] private int _healAmount;
	[SerializeField] private float _scaleFactor = 1f;

	private void Start() {
		transform.localScale = new Vector3(
			_scaleFactor * transform.localScale.x,
			_scaleFactor * transform.localScale.y,
			_scaleFactor * transform.localScale.z
		);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (((1 << other.gameObject.layer) & _targetLayerMask) != 0) {
			if (other.TryGetComponent<IHealable>(out IHealable healable)) {
				healable.Heal(_healAmount);
				Destroy(gameObject);
			}
		}
	}
}
