using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy.Boss.Projectiles
{
	public class BossIceSpike : MonoBehaviour
	{
		[SerializeField] private float speed;
		[Space]
		[SerializeField] private AudioClip hitSFX;

		[HideInInspector] public Vector3 direction;

		private int damage;
		private bool itHitObject;

		private PlayerManager playerManager;
		private SpriteRenderer spriteRenderer;
		private AudioSource audioSource;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			audioSource = GetComponent<AudioSource>();

			damage = playerManager.defaultPlayerHealth / 2;
		}

		void Update()
		{
			transform.position += direction * speed * Time.deltaTime;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject || playerManager == null) return;

			if (playerManager.playerCollider == collider)
			{
				playerManager.DamagePlayer(damage);
				playerManager.ChillPlayer(5f);

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
