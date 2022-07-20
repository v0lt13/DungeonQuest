using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Traps.Projectiles
{
	public class BossIceSpike : MonoBehaviour
	{
		[SerializeField] private float speed;
		[Space(10f)]
		[SerializeField] private AudioClip hitSFX;

		[HideInInspector] public Vector3 direction;

		private int damage;
		private bool itHitObject;

		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

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
				playerManager.ChillPlayer(5f);

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
