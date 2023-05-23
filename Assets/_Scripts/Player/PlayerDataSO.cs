using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerDataSO : ScriptableObject {
	[Header("Movement Info")]
	public float moveSpeed = 3f;

	[Header("Air Info")]
	public float jumpForce = 10f;
	public float onAirXVelocityModifier = 0.7f;
	public float coyoteTimerDuration;

	[Header("Dash Info")]
	public float dashSpeed;
	public float dashDuration;
	public float dashCooldownDuration;

	[Header("Slide Info")]
	public float slideSpeed;
	public float slideDuration;
	public float slideCooldownDuration;

	[Header("Crouch Info")]
	public float crouchCooldownDuration;

	[Header("Health info")]
	public int maxHealth;

	[Header("Slow Motion")]
	public float slowMotionDuration;
}
