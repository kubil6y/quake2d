using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RoamingEnemyDataSO : ScriptableObject {
	public float moveSpeed;

	[Header("Collision Check")]
	public float groundCheckDistance;
	public float wallCheckDistance;

	[Header("Attack Info")]
	public int attackDamage;
	public float attackCheckRadius;
	public float attackCooldown;

	[Header("Idle Info")]
	public float idleDuration;

	[Header("Health Info")]
	public int maxHealth;
}
