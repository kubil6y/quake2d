using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] private Texture2D _customCursor;

	private void Start() {
		Cursor.SetCursor(_customCursor, Vector3.zero, CursorMode.ForceSoftware);
	}
}
