using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Pickups
{
	public class ArmorPickup : MonoBehaviour
	{
		[SerializeField] private bool destroyOnPickup;

		private PlayerManager playerManager;
		private SpriteRenderer spriteRenderer;
		private Collider2D pickupCollider;
		private AudioSource audioSource;

		void Awake()
		{
			playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

			spriteRenderer = GetComponent<SpriteRenderer>();
			audioSource = GetComponent<AudioSource>();
			pickupCollider = GetComponent<Collider2D>();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerManager.GetComponent<Collider2D>())
			{
				// We return if the player armor is full
				if (playerManager.playerArmor == playerManager.defaultPlayerArmor) return;

				playerManager.ArmorPlayer(playerManager.defaultPlayerArmor);
				audioSource.Play();

				if (destroyOnPickup)
				{
					spriteRenderer.enabled = false;
					pickupCollider.enabled = false;
					Destroy(gameObject, 1f);
				}
			}
		}
	}
}
