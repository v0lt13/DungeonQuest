﻿using UnityEngine;

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
		[Space(10f)]
		[SerializeField] private int projectileDamage;
		[SerializeField] private float speed;

		private bool itHitObject;

		private GameObject player;
		private Animation fadeOutAnim;
		private Transform playerTransform;
		private Collider2D playerCollider;
		private Vector3 direction;

		void Awake()
		{
			fadeOutAnim = GetComponent<Animation>();

			player = GameObject.Find("Player");

			playerTransform = player.GetComponent<Transform>();
			playerCollider = player.GetComponent<Collider2D>();

			direction = (playerTransform.position - transform.position).normalized;

			// Make the projectile face the player
			float angle = Mathf.Atan2(direction.y, direction.x);
			transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90f);
		}

		void Update()
		{
			transform.position += direction * speed * Time.deltaTime;

			// Lock the z coordonate to 0
			var pos = transform.position;
			pos.z = 0;
			transform.position = pos;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject) return;

			if (collider == playerCollider)
			{
				collider.GetComponent<Player.PlayerManager>().DamagePlayer(projectileDamage);
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
