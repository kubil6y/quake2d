using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
	public enum Direction {
		Left = -1,
		Right = 1,
	}

	[Header("Life Time info")]
	[SerializeField] private int _maxHealth;
	[Tooltip("ghost will self destroy itself, if it can not attack lifeTimerMax seconds")]
	[SerializeField] private float _lifeTimeWithoutAttackTimerMax = 10f;

	[Header("Attack Info")]
	[SerializeField] private float _attackCooldown = 4f;
	[SerializeField] private float _attackDistance = 10f;

	[Header("Projectile Info")]
	[SerializeField] private float _spreadAngle = 30f;
	[SerializeField] private int _projectileAmount = 10;
	[SerializeField] private int _projectileDamage = 25;
	[SerializeField] private float _projectileMoveSpeed = 10f;
	[SerializeField] private float _projectileLifeTimerMax = 8f;

	[Header("Sine Movement")]
	[SerializeField] private float _moveSpeed = 2.0f;
	[SerializeField] private float _frequency = 10.0f;
	[SerializeField] private float _magnitude = 0.4f;


	private ObjectPool _projectileObjectPool;
	private ObjectPool _damagePopupObjectPool;
	private ObjectPool _bloodObjectPool;
	private ObjectPool _deathVFXObjectPool;

	private Direction _facingDirection;
	private Transform _playerTransform;
	private GhostStateMachine _stateMachine;
	private Health _health;
	private HealthEffects _healthEffects;
	private float _lifeTimeWithoutAttackTimer;

	public Animator animator { get; private set; }
	public CircleCollider2D circleCollider { get; private set; }

	private void Awake() {
		animator = GetComponentInChildren<Animator>();
		circleCollider = GetComponent<CircleCollider2D>();
		_health = GetComponent<Health>();
		_healthEffects = GetComponent<HealthEffects>();

		_facingDirection = Direction.Left;
		_stateMachine = new GhostStateMachine(this, _playerTransform);
		_stateMachine.TransitionTo(_stateMachine.appearState);
	}

	public void Setup(
		ObjectPool projectileObjectPool,
		ObjectPool damagePopupObjectPool,
		ObjectPool bloodObjectPool,
		ObjectPool deathVFXObjectPool) {
		_projectileObjectPool = projectileObjectPool;
		_damagePopupObjectPool = damagePopupObjectPool;
		_bloodObjectPool = bloodObjectPool;
		_deathVFXObjectPool = deathVFXObjectPool;
	}

	private void Start() {
		_playerTransform = Player.instance.transform;
		_health.Setup(_maxHealth);
		_healthEffects.Setup(_damagePopupObjectPool, _bloodObjectPool, _deathVFXObjectPool);

		_health.OnDied += Health_OnDied;
	}

	private void Health_OnDied(object sender, Vector3 e) {
		_stateMachine.TransitionTo(_stateMachine.deathState);
	}

	public void ResetLifeTimer() {
		_lifeTimeWithoutAttackTimer = 0;
	}

	private void Update() {
		_lifeTimeWithoutAttackTimer += Time.deltaTime;

		_stateMachine.Update();

		if (_lifeTimeWithoutAttackTimer > _lifeTimeWithoutAttackTimerMax) {
			DestroySelf();
		}
	}

	private void FixedUpdate() {
		_stateMachine.FixedUpdate();
	}

	#region getters/setters

	public float GetAttackCooldown() {
		return _attackCooldown;
	}

	public float GetAttackDistance() {
		return _attackDistance;
	}

	public float GetProjectileLifeTimerMax() {
		return _projectileLifeTimerMax;
	}

	public float GetProjectileMoveSpeed() {
		return _projectileMoveSpeed;
	}

	public int GetProjectileDamage() {
		return _projectileDamage;
	}

	public float GetSpreadAngle() {
		return _spreadAngle;
	}

	public float GetProjectileAmount() {
		return _projectileAmount; ;
	}

	public ObjectPool GetProjectileObjectPool() {
		return _projectileObjectPool;
	}

	public float GetGhostToPlayerAngleInDegrees() {
		Vector2 dir = GetGhostToPlayerDirectionNormalized();
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		return angle;
	}

	public Vector2 GetGhostToPlayerDirectionNormalized() {
		return (_playerTransform.position - transform.position).normalized;
	}

	public float GetDistanceToPlayer() {
		return Vector2.Distance(_playerTransform.position, transform.position);
	}

	public Direction GetFacingDirection() {
		return _facingDirection;
	}

	public int GetFacingDirectionValue() {
		return (int)_facingDirection;
	}

	public float GetMoveSpeed() {
		return _moveSpeed;
	}

	public float GetFrequency() {
		return _frequency;
	}

	public float GetMagnitude() {
		return _magnitude;
	}

	public void SetMoveSpeed(float moveSpeed) {
		_moveSpeed = moveSpeed;
	}

	public void SetFrequency(float frequency) {
		_frequency = frequency;
	}

	public void SetMagnitude(float magnitude) {
		_magnitude = magnitude;
	}

	#endregion // getters/setters

	public void AnimationFinished() {
		_stateMachine.AnimationFinished();
	}

	public void DestroySelf() {
		Destroy(gameObject);
	}

	public void FacePlayer() {
		if (_playerTransform == null) {
			return;
		}

		_facingDirection = Direction.Left;

		if (_playerTransform.transform.position.x < transform.position.x) {
			_facingDirection = Direction.Right;
		}

		Flip(_facingDirection);
	}

	private void Flip(Direction direction) {
		if (_facingDirection != direction) {
			transform.localScale = new Vector3(
				(int)_facingDirection * Mathf.Abs(transform.localScale.x),
				transform.localScale.y,
				transform.localScale.z
			);
		}
	}
}