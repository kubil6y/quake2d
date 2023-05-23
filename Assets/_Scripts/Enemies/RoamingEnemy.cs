using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingEnemy : MonoBehaviour {
	public event EventHandler<int> OnDirectionChanged;

	[SerializeField] private RoamingEnemyDataSO _roamingEnemyDataSO;
	[SerializeField] private Health _health;

	[System.Serializable]
	public enum State {
		Idle,
		Roaming,
		Death,
	}

	[Header("Collision Check")]
	[SerializeField] private Transform _groundCheckTransform;
	[SerializeField] private Transform _wallCheckTransform;
	[SerializeField] private LayerMask _groundLayerMask;

	[Header("Attack Info")]
	[SerializeField] private Transform _attackCheckTransform;
	[SerializeField] private LayerMask _playerLayerMask;
	private float _lastAttackTime;
	private float _idleTimer;

	private enum FacingDirection {
		Left = -1,
		Right = 1,
	}
	private FacingDirection _faceDir;

	[Space]
	public State _currentState;
	public State _previousState;

	private Animator animator;
	private Rigidbody2D rb;

	private readonly int KEY_IDLE = Animator.StringToHash("Idle");
	private readonly int KEY_ROAM = Animator.StringToHash("Roam");
	private readonly int KEY_DEATH = Animator.StringToHash("Death");

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();

		_faceDir = FacingDirection.Left;
		TransitionTo(State.Idle);
	}

	private void Start() {
		_health.Setup(_roamingEnemyDataSO.maxHealth);
		_health.OnDied += Health_OnDied;
	}

	private void Update() {
		switch (_currentState) {
			default:
			case State.Idle:
				CheckPlayerHit();
				HandleIdle();
				break;
			case State.Roaming:
				CheckPlayerHit();
				HandleRoaming();
				break;
			case State.Death:
				HandleDeath();
				break;
		}
	}

	private void HandleIdle() {
		_idleTimer -= Time.deltaTime;

		if (_idleTimer < 0) {
			TransitionTo(State.Roaming);
		}
	}

	private void HandleRoaming() {
		rb.velocity = new Vector2(_roamingEnemyDataSO.moveSpeed * (int)_faceDir, 0);

		if (!IsGrounded() || IsTouchingWall()) {
			TransitionTo(State.Idle);
			Flip();
		}
	}

	private void CheckPlayerHit() {
		Collider2D collider = Physics2D.OverlapCircle(_attackCheckTransform.position, _roamingEnemyDataSO.attackCheckRadius, _playerLayerMask);
		if (!collider) {
			return;
		}
		if (collider.TryGetComponent<Player>(out Player player)) {
			if (Time.time > _lastAttackTime + _roamingEnemyDataSO.attackCooldown) {
				_lastAttackTime = Time.time;
				player
					.GetComponent<IDamageable>()
					?.TakeDamage(_roamingEnemyDataSO.attackDamage, false);
			}
		}
	}

	private void HandleDeath() {
		Destroy(gameObject);
	}

	private void Enter(State state) {
		SetAnimation(state, true);

		switch (state) {
			default:
			case State.Idle:
				_idleTimer = _roamingEnemyDataSO.idleDuration;
				rb.velocity = Vector2.zero;
				break;
			case State.Roaming:
				break;
			case State.Death:
				break;
		}
	}

	private void Exit(State state) {
		SetAnimation(state, false);

		switch (state) {
			default:
			case State.Idle:
				_idleTimer = _roamingEnemyDataSO.idleDuration;
				break;
			case State.Roaming:
				break;
			case State.Death:
				break;
		}
	}

	private void TransitionTo(State newState) {
		Exit(_currentState);
		_previousState = _currentState;
		_currentState = newState;
		Enter(newState);
	}

	private void SetAnimation(State state, bool value) {
		animator.SetBool(GetAnimationHash(state), value);
	}

	private int GetAnimationHash(State state) {
		switch (state) {
			default:
			case State.Roaming:
				return KEY_ROAM;
			case State.Idle:
				return KEY_IDLE;
			case State.Death:
				return KEY_DEATH;
		}
	}

	private void Flip() {
		_faceDir = _faceDir == FacingDirection.Left ? FacingDirection.Right : FacingDirection.Left;

		OnDirectionChanged?.Invoke(this, -(int)_faceDir);

		transform.localScale = new Vector3(
			-(int)_faceDir * Mathf.Abs(transform.localScale.x),
			transform.localScale.y,
			transform.localScale.z
		);
	}

	public bool IsGrounded() {
		return Physics2D.Raycast(_groundCheckTransform.position, Vector2.down, _roamingEnemyDataSO.groundCheckDistance, _groundLayerMask);
	}

	public bool IsTouchingWall() {
		return Physics2D.Raycast(_wallCheckTransform.position, (int)_faceDir * Vector2.right, _roamingEnemyDataSO.wallCheckDistance, _groundLayerMask);
	}

	private void Health_OnDied(object sender, Vector3 position) {
		TransitionTo(State.Death);
	}

	private void OnDrawGizmos() {
		// Ground Check
		if (IsGrounded()) {
			Gizmos.color = Color.green;
		}
		Gizmos.DrawLine(_groundCheckTransform.position, new Vector3(_groundCheckTransform.position.x, _groundCheckTransform.position.y - _roamingEnemyDataSO.groundCheckDistance, 0));
		Gizmos.color = Color.white;

		// Wall Check
		if (IsTouchingWall()) {
			Gizmos.color = Color.green;
		}
		Gizmos.DrawLine(_wallCheckTransform.position, new Vector3(_wallCheckTransform.position.x + (int)_faceDir * _roamingEnemyDataSO.wallCheckDistance, _wallCheckTransform.position.y, 0));

		// Attack Check
		Gizmos.DrawWireSphere(_attackCheckTransform.position, _roamingEnemyDataSO.attackCheckRadius);
	}

}