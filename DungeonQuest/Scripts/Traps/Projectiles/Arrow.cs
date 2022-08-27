using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Traps.Projectiles
{
	public class Arrow : MonoBehaviour
	{
		[SerializeField] private float speed;
		[Space(10f)]
		[SerializeField] private AudioClip hitSFX;

		private int damage;
		private bool itHitObject;

		private Vector3 direction = new Vector3(0f, -1f, 0f);

		private Animator fadeOutAnim;
		private AudioSource audioSource;
		private PlayerManager playerManager;

		void Awake()
		{
			fadeOutAnim = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();

			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();			
			
			// Set damage to do 25% of the player's max health
			damage = playerManager.defaultPlayerHealth / 4;
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

				fadeOutAnim.Play("ProjectileFadeOut");
				Destroy(gameObject, 5f);
			}
		}
	}
}
