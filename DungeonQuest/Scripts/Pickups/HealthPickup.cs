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
