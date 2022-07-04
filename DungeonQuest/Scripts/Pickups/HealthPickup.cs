using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Pickups
{
	public class HealthPickup : MonoBehaviour
	{
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
				// We return if the player reached the potion cap
				if (playerManager.playerHealing.healingPotions == PlayerHealing.POTION_CAP) return;

				playerManager.playerHealing.AddPotions(1);
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
