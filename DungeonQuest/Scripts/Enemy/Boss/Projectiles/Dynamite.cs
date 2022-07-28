using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Enemy.Boss.Projectiles
{
	public class Dynamite : MonoBehaviour 
	{
		[SerializeField] private float speed;
		[SerializeField] private float explosionRange;
		[Space(10f)]
		[SerializeField] private AudioClip explosionSFX;

		[HideInInspector] public Vector3 direction;

		private int damage;
		private bool itHitObject;
		private bool hasExploded;

		private Animator animator;
		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
			animator = GetComponent<Animator>();
			
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
				transform.Rotate(new Vector3(0f, 0f, 10f + Time.deltaTime));
				Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);
			}
		}
		
		void OnTriggerEnter2D(Collider2D collider)
		{
			if (itHitObject || playerManager == null) return;

			if (playerManager.collider2D == collider)
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
			hasExploded = true;
			audio.clip = explosionSFX;
			audio.pitch = Random.Range(1f, 1.5f);

			audio.Play();
			animator.Play("Explosion");

			if (Vector2.Distance(transform.position, playerManager.transform.position) <= explosionRange) playerManager.DamagePlayer(damage);

			Destroy(gameObject, 5f);
		}
	}
}
