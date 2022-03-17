using UnityEngine;

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
		[SerializeField] private int projectileDamage;
		[SerializeField] private float speed;
		private bool itHitObject;

		private Animation fadeOutAnim;
		private Transform playerTransform;
		private Vector3 direction;


		void Awake()
		{
			fadeOutAnim = GetComponent<Animation>();

			playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

			direction = (playerTransform.position - transform.position).normalized;
			transform.up = playerTransform.position - transform.position;

			var pos = transform.rotation;
			pos.x = 0;
			pos.y = 0;
			transform.rotation = pos;
		}

		void Update()
		{
			transform.position += direction * speed * Time.deltaTime;

			var pos = transform.position;
			pos.z = 0;
			transform.position = pos;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject) return;

			if (collider.CompareTag("Player"))
			{
				collider.GetComponent<Player.PlayerManager>().DamagePlayer(projectileDamage);
				Destroy(gameObject);
			}

			if (collider.CompareTag("Blockable"))
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
