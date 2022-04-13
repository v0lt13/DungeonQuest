using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Pickups
{
	public class ArmorPickup : MonoBehaviour
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
				if (playerManager.playerArmor == playerManager.defaultPlayerArmor) return;

				playerManager.ArmorPlayer(playerManager.defaultPlayerArmor);
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
