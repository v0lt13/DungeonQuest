using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Traps.Projectiles
{
	public class IceSpike : MonoBehaviour
	{
		[SerializeField] private float speed;
		[Space(10f)]
		[SerializeField] private AudioClip hitSFX;

		private int damage;
		private bool itHitObject;

		private Vector3 direction = new Vector3(0f, -1f, 0f);

		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();			
			
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

			if (playerManager.collider2D == collider)
			{
				playerManager.DamagePlayer(damage);
				playerManager.ChillPlayer();

				Destroy(gameObject);
			}
			else if (collider.CompareTag("Enemy"))
			{
				collider.GetComponent<Enemy.EnemyManager>().DamageEnemy(damage);
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Blockable"))
			{
				audio.clip = hitSFX;
				audio.pitch = Random.Range(1f, 1.5f);
				audio.Play();

				itHitObject = true;
				direction = Vector2.zero;

				Destroy(renderer);
				Destroy(gameObject, 5f);
			}
		}
	}
}
