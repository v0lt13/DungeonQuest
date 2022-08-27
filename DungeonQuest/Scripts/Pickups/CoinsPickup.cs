using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Pickups
{
	public class CoinsPickup : MonoBehaviour
	{
		[SerializeField] private int amountGiven;
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
				// Return if the player has reached the coins cap, somehow
				if (playerManager.coinsAmount == PlayerManager.COINS_CAP) return;

				playerManager.GiveCoins(amountGiven);
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
