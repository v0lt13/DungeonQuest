﻿using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy
{
	public class EnemyProjectile : MonoBehaviour
	{
		public enum ProjectileType
		{
			Weapon,
			Snowball,
			Fireball
		}

		[Header("Projectile Config:")]
		public ProjectileType projectileType;
		[SerializeField] private float speed;
		[SerializeField] private AudioClip hitSFX;
		
		private bool itHitObject;

		private PlayerManager playerManager;
		private AudioSource audioSource;
		private Animator animator;
		private SpriteRenderer spriteRenderer;

		private Vector3 direction;

		public int ProjectileDamage { private get; set; }

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
			spriteRenderer = GetComponent<SpriteRenderer>();

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

			if (collider == playerManager.playerCollider)
			{
				playerManager.DamagePlayer(ProjectileDamage);

				if (projectileType == ProjectileType.Snowball)
				{
					playerManager.ChillPlayer(2f);
				}

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
					case ProjectileType.Weapon:
						animator.Play("ProjectileFadeOut");
						Destroy(gameObject, 5f);
						break;

					case ProjectileType.Fireball:
						if (Vector2.Distance(transform.position, playerManager.transform.position) <= 12) playerManager.DamagePlayer(ProjectileDamage);

						animator.Play("Explosion");
						Destroy(gameObject, 3f);
						break;

					default:
						spriteRenderer.enabled = false;
						Destroy(gameObject, 1f);
						break;
				}

				audioSource.clip = hitSFX;
				audioSource.pitch = Random.Range(1f, 1.5f);
				audioSource.Play();

				itHitObject = true;
				direction = Vector2.zero;
			}
		}
	}
}
