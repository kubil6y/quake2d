using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class ObjectPoolSetup : MonoBehaviour {
	[SerializeField] private int _poolSize;
	[SerializeField] PooledObject _pooledObject;
	private ObjectPool _objectPool;

	private void Awake() {
		_objectPool = GetComponent<ObjectPool>();
	}

	private void Start() {
		_objectPool.SetupPool(_pooledObject, _poolSize);
	}
}
