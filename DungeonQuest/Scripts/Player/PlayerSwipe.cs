using UnityEngine;
using DungeonQuest.Enemy;

namespace DungeonQuest.Player
{
	public class PlayerSwipe : MonoBehaviour
	{
		[SerializeField] private float knockbackPower;
		[SerializeField] private float stunTime;

		void Start()
		{
			Destroy(gameObject, 0.1f);
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.CompareTag("Enemy"))
			{
				var enemyManager = collider.GetComponent<EnemyManager>();
				var playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

				var damage = playerManager.playerAttack.damage;

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

				collider.GetComponent<EnemyAI>().StunEnemy(stunTime);
			}
			else if (collider.CompareTag("Breakeble"))
			{
				collider.GetComponent<BreakableObject>().BreakObject();
			}
		}
	}
}
