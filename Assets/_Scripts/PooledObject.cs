using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour {
	private ObjectPool pool;
	public ObjectPool Pool { get => pool; set => pool = value; }

	public void Release() {
		pool.ReturnToPool(this);
		gameObject.SetActive(false);
	}
}
