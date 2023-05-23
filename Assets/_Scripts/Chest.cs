using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable {
	[SerializeField] private AudioClip _openClip;
	public event EventHandler OnInteracted;


	private Animator _animator;
	private bool _isInteracted;

	private readonly int KEY_OPEN = Animator.StringToHash("Open");

	private void Awake() {
		_animator = GetComponentInChildren<Animator>();
	}

	public void Interact(object sender) {
		if (_isInteracted) {
			return;
		}

		_isInteracted = true;
		_animator.SetTrigger("Open");
		AudioSource.PlayClipAtPoint(_openClip, transform.position, .5f);
	}

	public void InvokeAnimationFinished() {
		OnInteracted?.Invoke(this, EventArgs.Empty);
	}
}
