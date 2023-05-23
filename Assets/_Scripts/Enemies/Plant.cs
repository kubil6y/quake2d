using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// plant will attack the player when it gets to certain range
// it will keep attacking until player is away from its range
public class Plant : MonoBehaviour {
	[SerializeField] private PlantEnemyDataSO _plantDataSO;
	public event EventHandler OnShoot;

	[System.Serializable]
	public enum State {
		Idle,
		Alert,
		Attack,
		Death,
	}

	[SerializeField] private ObjectPool _objectPool;
	[SerializeField] private Health _health;
	[SerializeField] private Transform _projectileSpawnTransform;

	private int _projectileDamage;
	private float _projectileMovespeed;
	private float _attackDistance;
	private float _attackCooldown;
	private float _attackTimer;

	private State _currentState;
	private State _previousState;
	private bool _animTriggerCalled;

	private Transform _playerTransform;
	private Animator _animator;

	private readonly int KEY_ATTACK = Animator.StringToHash("Attack");

	private void Awake() {
		_animator = GetComponentInChildren<Animator>();
	}

	private void Start() {
		_playerTransform = Player.instance.transform;
		_attackDistance = _plantDataSO.attackDistance;
		_attackCooldown = _plantDataSO.attackCooldown;
		_projectileDamage = _plantDataSO.projectileDamage;
		_projectileMovespeed = _plantDataSO.projectileMoveSpeed;

		_health.Setup(_plantDataSO.maxHealth);
		_health.OnDied += Health_OnDied;
	}

	private void Update() {
		_attackTimer -= Time.deltaTime;
		switch (_currentState) {
			default:
			case State.Idle:
				HandleIdleState();
				break;
			case State.Alert:
				HandleAlertState();
				break;
			case State.Attack:
				HandleAttackState();
				break;
			case State.Death:
				HandleDeathState();
				break;
		}
	}

	private void TransitionTo(State newState) {
		_previousState = _currentState;
		_currentState = newState;
	}

	private void HandleIdleState() {
		if (IsPlayerInRange()) {
			TransitionTo(State.Alert);
		}
	}

	private void HandleAlertState() {
		_animTriggerCalled = false;

		if (!IsPlayerInRange()) {
			TransitionTo(State.Idle);
		}

		if (_attackTimer < 0) {
			TransitionTo(State.Attack);
		}
	}

	private void HandleAttackState() {
		if (!IsPlayerInRange() || _animTriggerCalled) {
			TransitionTo(State.Idle);
		}

		if (_attackTimer < 0) {
			OnShoot?.Invoke(this, EventArgs.Empty);

			_animator.SetTrigger(KEY_ATTACK);
			_attackTimer = _attackCooldown;
			SpawnProjectile();

			TransitionTo(State.Alert);
		}
	}

	private void SpawnProjectile() {
		GameObject projectileObject = _objectPool.GetPooledObject().gameObject;
		projectileObject.transform.position = _projectileSpawnTransform.transform.position;
		Quaternion projectileRotation = Quaternion.Euler(new Vector3(0, 0, -180));
		projectileObject.transform.rotation = projectileRotation;

		if (projectileObject.TryGetComponent<Projectile>(out Projectile projectile)) {
			float lifeTimerMax = 4f;
			projectile.Setup(_projectileDamage, _projectileMovespeed, false, lifeTimerMax);
		}
	}

	private void HandleDeathState() {
		Destroy(gameObject);
	}

	public void AnimationFinishedEvent() {
		_animTriggerCalled = true;
	}

	private bool IsPlayerInRange() {
		float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
		return distanceToPlayer < _attackDistance;
	}

	private void Health_OnDied(object sender, Vector3 position) {
		TransitionTo(State.Death);
	}
}
