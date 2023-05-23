using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponDataSO : ScriptableObject {

	public string weaponName;
	public bool isSingleFire;
	public int bulletAmount = 1;
	public int spreadAngle;
	public int damage;
	public int damageRange;
	[Range(0, 1f)]
	public float critChance;
	public float delayBetweenRounds;
	public float projectileSpeed;
	public int ammoCapacity;
	public AudioClip shotFiredClip;
}