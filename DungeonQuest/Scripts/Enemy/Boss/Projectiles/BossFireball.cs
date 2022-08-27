using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy.Boss.Projectiles
{
	public class BossFireball : MonoBehaviour 
	{
		[SerializeField] private float speed;
		[Space(10f)]
		[SerializeField] private AudioClip hitSFX;

		[HideInInspector] public Vector3 direction;

		private int damage;
		private bool itHitObject;

		private PlayerManager playerManager;
		private Animator animator;
		private AudioSource audioSource;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();

			damage = playerManager.defaultPlayerHealth / 3;
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
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Blockable"))
			{
				audioSource.clip = hitSFX;
				audioSource.pitch = Random.Range(1f, 1.5f);
				audioSource.Play();
				animator.Play("Explosion");

				itHitObject = true;
				direction = Vector2.zero;

				if (Vector2.Distance(transform.position, playerManager.transform.position) <= 20) playerManager.DamagePlayer(damage);

				Destroy(gameObject, 3f);
			}
		}
	}
}
