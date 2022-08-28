using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Traps.Projectiles
{
	public class IceSpike : MonoBehaviour
	{
		[SerializeField] private float speed;
		[Space]
		[SerializeField] private AudioClip hitSFX;

		private int damage;
		private bool itHitObject;

		private Vector3 direction = new Vector3(0f, -1f, 0f);

		private PlayerManager playerManager;
		private AudioSource audioSource;
		private SpriteRenderer spriteRenderer;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
			audioSource = GetComponent<AudioSource>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			
			// Set damage to do 15% of the player's max health
			damage = playerManager.defaultPlayerHealth / 8;
		}

		void Update()
		{
			// Move downwards
			transform.position += direction * speed * Time.deltaTime;			
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject || playerManager == null) return;

			if (playerManager.playerCollider == collider)
			{
				playerManager.DamagePlayer(damage);
				playerManager.ChillPlayer(3f);

				Destroy(gameObject);
			}
			else if (collider.CompareTag("Enemy"))
			{
				collider.GetComponent<Enemy.EnemyManager>().DamageEnemy(damage);
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Blockable"))
			{
				audioSource.clip = hitSFX;
				audioSource.pitch = Random.Range(1f, 1.5f);
				audioSource.Play();

				itHitObject = true;
				direction = Vector2.zero;

				Destroy(spriteRenderer);
				Destroy(gameObject, 5f);
			}
		}
	}
}
