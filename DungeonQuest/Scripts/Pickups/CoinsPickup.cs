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
			if (playerManager.collider2D == collider)
			{
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
