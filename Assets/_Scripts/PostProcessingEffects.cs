using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingEffects : MonoBehaviour {
	[SerializeField] private UnityEngine.Rendering.Volume _volume;

	private UnityEngine.Rendering.VolumeProfile _volumeProfile;
	private UnityEngine.Rendering.Universal.SplitToning _splitToning;
	private UnityEngine.Rendering.Universal.FilmGrain _filmGrain;
	private UnityEngine.Rendering.Universal.WhiteBalance _whiteBalance;

	private Player _player;

	private void Start() {
		_player = Player.instance;
		_volumeProfile = _volume.profile;

		if (!_volumeProfile.TryGet(out _splitToning)) {
			Debug.LogWarning("VolumeProfile:SplitToning is not found");
		}
		if (!_volumeProfile.TryGet(out _filmGrain)) {
			Debug.LogWarning("VolumeProfile:FilmGrain is not found");
		}
		if (!_volumeProfile.TryGet(out _whiteBalance)) {
			Debug.LogWarning("VolumeProfile:WhiteBalance is not found");
		}

		_player.OnSlowMotionStarted += Player_OnSlowMotionStarted;
		_player.OnSlowMotionEnded += Player_OnSlowMotionEnded;
		_player.OnQuadStarted += Player_OnQuadStarted;
		_player.OnQuadStopped += Player_OnQuadStopped;
	}

	private void Player_OnSlowMotionStarted(object sender, EventArgs e) {
		Time.timeScale = .60f;
		_filmGrain.active = true;
		_whiteBalance.active = true;
	}

	private void Player_OnSlowMotionEnded(object sender, EventArgs e) {
		_whiteBalance.active = false;
		_filmGrain.active = false;
		Time.timeScale = 1f;
	}

	private void Player_OnQuadStarted(object sender, float seconds) {
		_splitToning.active = true;
	}

	private void Player_OnQuadStopped(object sender, EventArgs e) {
		_splitToning.active = false;
	}
}
