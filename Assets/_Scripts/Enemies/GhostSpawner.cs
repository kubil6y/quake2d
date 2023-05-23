using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour {
	[SerializeField] private GameObject _ghostPrefab;
	[SerializeField] private float _spawnCooldown;

	[Header("Object Pools")]
	[SerializeField] private ObjectPool _projectileObjectPool;
	[SerializeField] private ObjectPool _damagePopupObjectPool;
	[SerializeField] private ObjectPool _bloodObjectPool;
	[SerializeField] private ObjectPool _deathVFXObjectPool;
	[SerializeField] private float _firstGhostSpawnDelay = 10f;

	private Transform _playerTransform;
	private float _spawnTimer;
	private Camera _camera;

	private void Start() {
		_camera = Camera.main;
		_playerTransform = Player.instance.transform;
		_spawnTimer = _firstGhostSpawnDelay;
	}

	private void Update() {
		_spawnTimer -= Time.deltaTime;

		if (_spawnTimer < 0) {
			_spawnTimer = _spawnCooldown;
			SpawnGhost();
		}
	}

	private void SpawnGhost() {
		// TODO fix ghost spawn position
		// Vector3 pos = _camera.ViewportToScreenPoint(new Vector3(1, 1, _camera.nearClipPlane));
		// Vector3 pos = Camera.main.ScreenToViewportPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight, 0));
		GameObject ghostObject = Instantiate(_ghostPrefab, GetSpawnPosition(), Quaternion.identity);
		if (ghostObject.TryGetComponent<Ghost>(out Ghost ghost)) {
			ghost.Setup(_projectileObjectPool, _damagePopupObjectPool, _bloodObjectPool, _deathVFXObjectPool);
		}
	}

	private Vector3 GetSpawnPosition() {
		float x = _playerTransform.position.x + 15f;
		float y = UnityEngine.Random.Range(_playerTransform.position.y + 2f, _playerTransform.position.y + 6f);
		return new Vector3(x, y, 0);
	}
}
