using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlantEnemyDataSO : ScriptableObject {
	[Header("Attack Info")]
	public int projectileDamage;
	public float projectileMoveSpeed;

	public float attackDistance;
	public float attackCooldown;

	[Header("Health Info")]
	public int maxHealth;
}
