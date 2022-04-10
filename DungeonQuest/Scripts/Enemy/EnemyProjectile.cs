using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy
{
	public class EnemyProjectile : MonoBehaviour
	{
		public enum ProjectileType
		{
			Arrow,
			Other
		}

		[Header("Projectile Config:")]
		public ProjectileType projectileType;
		[SerializeField] private float speed;

		private bool itHitObject;

		private PlayerManager playerManager;
		private Animation fadeOutAnim;
		private Vector3 direction;

		public int ProjectileDamage { get; set; }

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

			fadeOutAnim = GetComponent<Animation>();

			direction = (playerManager.transform.position - transform.position).normalized;

			// Make the projectile face the player
			float angle = Mathf.Atan2(direction.y, direction.x);
			transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90f);
		}

		void Update()
		{
			// Move towards the player
			transform.position += direction * speed * Time.deltaTime;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject) return;

			if (collider == playerManager.collider2D)
			{
				playerManager.DamagePlayer(ProjectileDamage);
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Breakeble"))
			{
				collider.GetComponent<BreakableObject>().BreakObject();
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Blockable"))
			{
				switch (projectileType)
				{
					case ProjectileType.Arrow:
						fadeOutAnim.Play();
						Destroy(gameObject, 5f);
						break;

					case ProjectileType.Other:
						Destroy(gameObject);
						break;
				}

				itHitObject = true;
				direction = Vector2.zero;
			}
		}
	}
}
