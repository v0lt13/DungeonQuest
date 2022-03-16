using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Pickups
{
	public class ArmorPickup : MonoBehaviour
	{
		[SerializeField] private int amountGiven;
		[SerializeField] private bool destroyOnPickup;

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.CompareTag("Player"))
			{
				var player = collider.GetComponent<PlayerManager>();

				if (player.PlayerArmor == player.defaultPlayerArmor) return;
					
				player.ArmorPlayer(amountGiven);

				if (destroyOnPickup) Destroy(gameObject);
			}
		}
	}
}
