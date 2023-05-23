using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	private static T _instance;

	public static T instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<T>();
				if (_instance == null) {
					_instance = new GameObject("Instance of " + typeof(T)).AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake() {
		if (_instance != null) {
			Destroy(this.gameObject);
		}
	}
}


/*
public class Singleton<T> : MonoBehaviour where T : Component {
	private static T _instance;
	public static T Instance {
		get {
			if (_instance == null) {
				_instance = (T)FindObjectOfType(typeof(T));

				if (_instance == null) {
					SetupInstance();
				} else {
					string typeName = typeof(T).Name;

					Debug.Log("[Singleton] " + typeName + " instance already created: " +
						_instance.gameObject.name);
				}
			}

			return _instance;
		}
	}

	public virtual void Awake() {
		RemoveDuplicates();

	}

	private static void SetupInstance() {
		// lazy instantiation
		_instance = (T)FindObjectOfType(typeof(T));

		if (_instance == null) {
			GameObject gameObj = new GameObject();
			gameObj.name = typeof(T).Name;

			_instance = gameObj.AddComponent<T>();
			DontDestroyOnLoad(gameObj);
		}
	}

	private void RemoveDuplicates() {
		if (_instance == null) {
			_instance = this as T;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
*/