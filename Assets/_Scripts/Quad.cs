using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour {
	[SerializeField] private float _duration;
	[SerializeField] private AudioClip _quadAnnouncerClip;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<Player>(out Player player)) {
			player.GetQuad(_duration);

			AudioSource.PlayClipAtPoint(_quadAnnouncerClip, transform.position);

			Destroy(gameObject);
		}
	}
}
