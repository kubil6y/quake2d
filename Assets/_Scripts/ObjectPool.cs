using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
	// initial number of cloned objects
	 private int initPoolSize;

	// PooledObject prefab
	 private PooledObject objectToPool;

	// store the pooled objects in stack
	private Stack<PooledObject> stack;

	// creates the pool (invoke when the lag is not noticeable)
	public void SetupPool(PooledObject pooledObject, int poolSize) {
		objectToPool = pooledObject;
		initPoolSize = poolSize;

		// missing objectToPool Prefab field
		if (objectToPool == null) {
			return;
		}

		stack = new Stack<PooledObject>();

		// populate the pool
		PooledObject instance = null;

		for (int i = 0; i < initPoolSize; i++) {
			instance = Instantiate(objectToPool, transform);
			instance.Pool = this;
			instance.gameObject.SetActive(false);
			stack.Push(instance);
		}
	}

	// returns the first active GameObject from the pool
	public PooledObject GetPooledObject() {
		// missing objectToPool field
		if (objectToPool == null) {
			return null;
		}

		// if the pool is not large enough, instantiate extra PooledObjects
		if (stack.Count == 0) {
			PooledObject newInstance = Instantiate(objectToPool);
			newInstance.Pool = this;
			return newInstance;
		}

		// otherwise, just grab the next one from the list
		PooledObject nextInstance = stack.Pop();
		nextInstance.gameObject.SetActive(true);
		return nextInstance;
	}

	public void ReturnToPool(PooledObject pooledObject) {
		stack.Push(pooledObject);
		pooledObject.gameObject.SetActive(false);
	}
}
