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

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
			animator = GetComponent<Animator>();

			damage = playerManager.defaultPlayerHealth / 3;
		}

		void Update()
		{
			transform.position += direction * speed * Time.deltaTime;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject || playerManager == null) return;

			if (playerManager.collider2D == collider)
			{
				playerManager.DamagePlayer(damage);
				Destroy(gameObject);
			}
			else if (collider.CompareTag("Blockable"))
			{
				audio.clip = hitSFX;
				audio.pitch = Random.Range(1f, 1.5f);
				audio.Play();
				animator.Play("Explosion");

				itHitObject = true;
				direction = Vector2.zero;

				if (Vector2.Distance(transform.position, playerManager.transform.position) <= 20) playerManager.DamagePlayer(damage);

				Destroy(gameObject, 3f);
			}
		}
	}
}
