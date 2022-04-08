using UnityEngine;
using DungeonQuest.Enemy;

namespace DungeonQuest.Player
{
	public class PlayerSwipe : MonoBehaviour
	{
		[SerializeField] private float knockbackPower;

		private const float KNOCKBACK_DURATION = 0.1f;

		private bool itHit;

		void Start()
		{
			Destroy(gameObject, 0.1f);
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHit) return; // Sometime this gets called more then once in a singular frame, so we make sure it happenes only once

			if (collider.CompareTag("Enemy"))
			{
				var enemyManager = collider.GetComponent<EnemyManager>();
				var playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

				var damage = playerManager.playerAttack.GetDamage;

				if (enemyManager == null) return;

				var healthDifference = enemyManager.GetEnemyHealth - damage;

				// We check if the enemy is gonna die on the next hit, then we give the player XP if is true
				if (healthDifference <= 0)
				{
					playerManager.playerLeveling.PlayerXP += Random.Range(enemyManager.enemyDrops.GetMinXpDrop, enemyManager.enemyDrops.GetMaxXpDrop);					
				}

				Vector2 difference = (enemyManager.transform.position - playerManager.transform.position).normalized * knockbackPower;

				enemyManager.DamageEnemy(damage);
				enemyManager.rigidbody2D.AddForce(difference, ForceMode2D.Impulse);

				collider.GetComponent<EnemyAI>().StunEnemy(KNOCKBACK_DURATION);

				itHit = true;
			}
			else if (collider.CompareTag("Breakeble"))
			{
				collider.GetComponent<BreakableObject>().BreakObject();
			}
		}
	}
}
