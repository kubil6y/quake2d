using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
// TODO: make this class more useful
public class GameAssets : MonoBehaviour {
	private static GameAssets _instance;
	public static GameAssets instance {
		get {
			if (_instance == null) {
				_instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
			}
			return _instance;
		}
	}

	public Transform damagePopupTf;
}

*/

public class GameAssets : Singleton<GameAssets> {
	public Transform damagePopupTf;
}