using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : Singleton<Logger> {
	[SerializeField] private string _prefix;
	[SerializeField] private bool _showLogs;
	[SerializeField] private Color _color;
	private string _hexColor;

	protected override void Awake() {
		base.Awake();
		_hexColor = ColorUtility.ToHtmlStringRGB(_color);
	}

	public void Log(string message, Object context = null) {
		if (!_showLogs) {
			return;
		}
		Debug.Log($"<color=#{_hexColor}>{_prefix}</color>: {message}", context);
	}
}
