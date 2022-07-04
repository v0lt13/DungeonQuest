using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Pickups
{
	public class CoinsPickup : MonoBehaviour
	{
		[SerializeField] private int amountGiven;
		[SerializeField] private bool destroyOnPickup;

		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider == playerManager.collider2D)
			{
				// Return if the player has reached the coins cap, somehow
				if (playerManager.coinsAmount == PlayerManager.COINS_CAP) return;

				playerManager.GiveCoins(amountGiven);
				audio.Play();

				if (destroyOnPickup)
				{
					renderer.enabled = false;
					collider2D.enabled = false;
					Destroy(gameObject, 1f);
				}
			}
		}
	}
}
