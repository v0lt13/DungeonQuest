using UnityEngine;
using System.Collections;
using DungeonQuest.Enemy;

namespace DungeonQuest.Player
{
	public class SwipeAttack : MonoBehaviour
	{
		private const float KNOCKBACK_DURATION = 0.1f;
		private const float KNOCKBACK_POWER = 200f;

		void Start()
		{
			Destroy(gameObject, 0.1f);
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Enemy"))
			{
				var enemy = other.GetComponent<EnemyManager>();
				var damage = GameObject.FindObjectOfType<PlayerAttack>().damage;
				var player = GameObject.Find("Player");

				Vector2 difference = (enemy.transform.position - player.transform.position).normalized * KNOCKBACK_POWER;

				enemy.DamageEnemy(damage);
				enemy.rigidbody2D.AddForce(difference, ForceMode2D.Impulse);

				other.GetComponent<EnemyAI>().StunTime = KNOCKBACK_DURATION;
			}
		}
	}
}
