using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>, IHealable {
	public event EventHandler<bool> OnPlayerCanShoot;
	public event EventHandler<int> OnDirectionChanged;
	public event EventHandler OnSlowMotionStarted;
	public event EventHandler OnSlowMotionEnded;
	public event EventHandler<float> OnQuadStarted;
	public event EventHandler OnQuadStopped;

	[SerializeField] PlayerDataSO _playerDataSO;
	[SerializeField] private WeaponManager _weaponManager;
	public Health health;
	[SerializeField] public Logger logger;

	[Header("Collision Check")]
	[SerializeField] private LayerMask _groundLayerMask;
	[SerializeField] private LayerMask _interactableLayerMask;
	[SerializeField] private Transform _groundCheckTransform;
	[SerializeField] private Transform _wallCheckTransform;
	[SerializeField] private Transform _interactionTransform;
	[SerializeField] private float _groundCheckDistance;
	[SerializeField] private float _wallCheckDistance;
	[SerializeField] private float _interactionDistance;

	public Animator animator { get; private set; }
	public Rigidbody2D rb { get; private set; }
	private CircleCollider2D _circleCollider;
	private CapsuleCollider2D _capsuleCollider;
	private SpriteRenderer _spriteRenderer;

	public enum Collider {
		Full,
		Half,
	}

	private int _damageMultiplier = 1;
	private int _faceDir = 1;
	private bool _isSlowMotion;
	private bool _canSlowMo = true;
	private bool _canShoot = true;

	private Vector2 _input;
	private PlayerStateMachine stateMachine;

	private readonly int KEY_VELOCITY_Y = Animator.StringToHash("VelocityY");

	protected override void Awake() {
		base.Awake();

		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		_capsuleCollider = GetComponent<CapsuleCollider2D>();
		_circleCollider = GetComponent<CircleCollider2D>();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		stateMachine = new PlayerStateMachine(this);
		stateMachine.TransitionTo(stateMachine.idleState);

		SetCollider(Collider.Full);
	}

	private void Start() {
		health.Setup(_playerDataSO.maxHealth);
		health.OnDied += Player_OnDied;
	}

	private void Update() {
		SetInputNormalized();
		MouseFollow();
		HandleInput();

  
		if (Input.GetKeyDown(KeyCode.K)) {
			Revive(Vector3.zero);
		}
		

		animator.SetFloat(KEY_VELOCITY_Y, rb.velocity.y);
		stateMachine.currentState.Update();
	}

	private void Player_OnDied(object sender, Vector3 position) {
		Die();
	}

	// TODO death state player
	public void Die() {
		stateMachine.TransitionTo(stateMachine.deathState);
	}

	public void Revive(Vector3 position) {
		transform.position = position;
		stateMachine.TransitionTo(stateMachine.idleState);
		health.SetFullHealth();
	}

	public void GetQuad(float seconds) {
		StartCoroutine(QuadRoutine(seconds));
	}

	private void HandleInput() {
		// TODO remove KeyCode.T
		if (Input.GetKeyDown(KeyCode.T)) {
			transform.position = new Vector2(0, 3);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0) && CanShoot()) {
			_weaponManager.Shoot();
		}

		if (Input.GetKeyUp(KeyCode.Mouse0)) {
			_weaponManager.StopShooting();
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			CheckInteraction();
		}
	}

	private void CheckInteraction() {
		RaycastHit2D hit = Physics2D.Raycast(_interactionTransform.position, Vector2.right, _interactionDistance, _interactableLayerMask);

		if (!hit) {
			return;
		}

		if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable)) {
			interactable.Interact(this);
		}
	}

	private void MouseFollow() {
		if (!health.IsAlive()) {
			return;
		}
		var pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pointerPos.z = 0;
		var aimDirection = (Vector3)pointerPos - transform.position;

		// flip weapon on y
		float desiredAngle =
			Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

		if (desiredAngle > 90 || desiredAngle < -90) {
			FlipWithDirection(-1);
		} else {
			FlipWithDirection(1);
		}
	}

	private void FixedUpdate() {
		stateMachine.currentState.FixedUpdate();
	}

	#region getters/setters


	public int GetDamageMultiplier() {
		return _damageMultiplier;
	}

	public bool CanShoot() {
		return _canShoot;
	}

	public bool CanSlowMo() {
		return _canSlowMo;
	}

	public void SetCanShoot(bool canShoot) {
		OnPlayerCanShoot?.Invoke(this, canShoot);
		_canShoot = canShoot;
	}

	public IEnumerator QuadRoutine(float seconds) {
		_damageMultiplier = 4;
		OnQuadStarted?.Invoke(this, seconds);

		yield return new WaitForSeconds(seconds);

		_damageMultiplier = 1;
		OnQuadStopped?.Invoke(this, EventArgs.Empty);
	}

	public IEnumerator SlowMotionRoutine() {
		_isSlowMotion = true;
		OnSlowMotionStarted?.Invoke(this, EventArgs.Empty);

		yield return new WaitForSeconds(_playerDataSO.slowMotionDuration);

		_isSlowMotion = false;
		OnSlowMotionEnded?.Invoke(this, EventArgs.Empty);
	}

	public bool IsSlowMotion() {
		return _isSlowMotion;
	}

	public int GetFaceDir() {
		return _faceDir;
	}

	public Vector2 GetInputNormalized() {
		return _input;
	}

	public float GetInputX() {
		return _input.x;
	}

	public float GetInputY() {
		return _input.y;
	}

	private void SetInputNormalized() {
		_input = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")).normalized;
	}

	public float GetMovementSpeed() {
		return _playerDataSO.moveSpeed;
	}

	public float GetJumpForce() {
		return _playerDataSO.jumpForce;
	}

	public float GetOnAirXVelocityModifier() {
		return _playerDataSO.onAirXVelocityModifier;
	}

	public float GetCoyoteTimerDuration() {
		return _playerDataSO.coyoteTimerDuration;
	}

	public float GetDashSpeed() {
		return _playerDataSO.dashSpeed;
	}

	public float GetDashDuration() {
		return _playerDataSO.dashDuration;
	}

	public float GetDashCooldownDuration() {
		return _playerDataSO.dashCooldownDuration;
	}

	public float GetSlideSpeed() {
		return _playerDataSO.slideSpeed;
	}

	public float GetSlideDuration() {
		return _playerDataSO.slideDuration;
	}

	public float GetSlideCooldownDuration() {
		return _playerDataSO.slideCooldownDuration;
	}

	public float GetCrouchCooldownDuration() {
		return _playerDataSO.crouchCooldownDuration;
	}

	#endregion // getters/setters

	#region velocity and flips
	public void SetVelocity(float velocityX, float velocityY) {
		rb.velocity = new Vector2(velocityX, velocityY);
		CheckForFlip(rb.velocity.x);
	}

	public void SetVelocity(Vector2 velocity) {
		rb.velocity = velocity;
		CheckForFlip(rb.velocity.x);
	}

	public void SetVelocityZero() {
		rb.velocity = Vector2.zero;
	}

	public void CheckForFlip(float velocityX) {
		float faceDir = GetFaceDir();
		if (velocityX > 0 && faceDir == -1 ||
			velocityX < 0 && faceDir == 1) {
			Flip();
		}
	}

	public void Flip() {
		FlipFaceDir();
		transform.localScale = new Vector3(
			GetFaceDir() * Mathf.Abs(transform.localScale.x),
			transform.localScale.y,
			transform.localScale.z
		);
	}

	public void FlipWithDirection(int direction) {
		OnDirectionChanged?.Invoke(this, direction);
		transform.localScale = new Vector3(
			direction * Mathf.Abs(transform.localScale.x),
			transform.localScale.y,
			transform.localScale.z
		);
	}

	private void FlipFaceDir() {
		_faceDir *= -1;
	}


	#endregion // velocity and flips

	#region physic checks

	public bool IsGrounded() {
		return Physics2D.Raycast(_groundCheckTransform.position, Vector2.down, _groundCheckDistance, _groundLayerMask);
	}

	public bool IsTouchingWall() {
		return Physics2D.Raycast(_wallCheckTransform.position, GetFaceDir() * Vector2.right, _wallCheckDistance, _groundLayerMask);
	}

	public void SetCollider(Player.Collider collider) {
		switch (collider) {
			case Player.Collider.Half:
				_circleCollider.enabled = true;
				_capsuleCollider.enabled = false;
				break;
			case Player.Collider.Full:
				_circleCollider.enabled = false;
				_capsuleCollider.enabled = true;
				break;
		}
	}

	#endregion // physic checks


	private void OnDrawGizmos() {
		// Grounded Gizmos
		if (IsGrounded()) {
			Gizmos.color = Color.green;

		}
		Gizmos.DrawLine(_groundCheckTransform.position, new Vector3(_groundCheckTransform.position.x, _groundCheckTransform.position.y - _groundCheckDistance, 0));
		Gizmos.color = Color.white;

		// Wall Gizmos
		if (IsTouchingWall()) {
			Gizmos.color = Color.green;
		}

		Gizmos.DrawLine(_wallCheckTransform.position, new Vector3(_wallCheckTransform.position.x + GetFaceDir() * _wallCheckDistance, _wallCheckTransform.position.y, 0));
		Gizmos.color = Color.white;

		// Interactable Gizmos
		Gizmos.DrawLine(_interactionTransform.position, new Vector3(_interactionTransform.position.x + GetFaceDir() * _interactionDistance, _interactionTransform.position.y, 0));
	}

	public void Heal(int amount) {
		health.Heal(amount);
	}
}
