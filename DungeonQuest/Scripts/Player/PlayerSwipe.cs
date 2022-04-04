using UnityEngine;
using DungeonQuest.Enemy;

namespace DungeonQuest.Player
{
	public class PlayerSwipe : MonoBehaviour
	{
		[SerializeField] private float knockbackPower;

		private const float KNOCKBACK_DURATION = 0.1f;

		void Start()
		{
			Destroy(gameObject, 0.1f);
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.CompareTag("Enemy"))
			{
				var enemy = collider.GetComponent<EnemyManager>();
				var damage = GameObject.FindObjectOfType<PlayerAttack>().GetDamage;
				var player = GameObject.Find("Player");

				if (enemy == null) return;

				Vector2 difference = (enemy.transform.position - player.transform.position).normalized * knockbackPower;

				enemy.DamageEnemy(damage);
				enemy.rigidbody2D.AddForce(difference, ForceMode2D.Impulse);

				collider.GetComponent<EnemyAI>().StunEnemy(KNOCKBACK_DURATION);
			}
		}
	}
}
