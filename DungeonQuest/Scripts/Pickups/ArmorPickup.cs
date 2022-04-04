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
			if (playerManager.collider2D == collider)
			{
				if (playerManager.PlayerArmor == playerManager.GetDefaultPlayerArmor) return;

				playerManager.ArmorPlayer(playerManager.GetDefaultPlayerArmor);
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
