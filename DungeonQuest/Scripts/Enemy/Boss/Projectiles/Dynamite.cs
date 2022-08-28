
using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy.Boss.Projectiles
{
	public class Dynamite : MonoBehaviour 
	{
		[SerializeField] private float speed;
		[SerializeField] private float explosionRange;
		[Space]
		[SerializeField] private AudioClip explosionSFX;

		[HideInInspector] public Vector3 direction;

		private int damage;
		private bool itHitObject;
		private bool hasExploded;

		private Animator animator;
		private AudioSource audioSource;
		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
			
			damage = playerManager.defaultPlayerHealth / 2;
		}

		void Update()
		{
			if (transform.position == direction)
			{
				Explode();
			}
			else if (!hasExploded)
			{
				transform.Rotate(new Vector3(0f, 0f, 500f * Time.deltaTime));
				transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);				
			}
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject || playerManager == null) return;

			if (playerManager.playerCollider == collider)
			{
				Explode();
			}

			if (collider.CompareTag("Blockable"))
			{
				itHitObject = true;
				direction = Vector2.zero;

				Explode();
			}
		}

		private void Explode()
		{
			if (hasExploded) return;

			hasExploded = true;
			audioSource.clip = explosionSFX;
			audioSource.pitch = Random.Range(1f, 1.5f);

			audioSource.Play();
			animator.Play("Explosion");

			if (Vector2.Distance(transform.position, playerManager.transform.position) <= explosionRange) playerManager.DamagePlayer(damage);

			Destroy(gameObject, 3f);
		}
	}
}
