using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class ParticleVFX : MonoBehaviour {
	private PooledObject _pooledObject;
	private ParticleSystem _particleSystem;

	private void Awake() {
		_pooledObject = GetComponent<PooledObject>();
	}

	private void Start() {
		_particleSystem = GetComponent<ParticleSystem>();
		var main = _particleSystem.main;
		main.stopAction = ParticleSystemStopAction.Callback;
	}

	public void SetupAndPlay(Vector3 position) {
		transform.position = position;
	}

	private void OnParticleSystemStopped() {
		_pooledObject.Release();
	}
}
