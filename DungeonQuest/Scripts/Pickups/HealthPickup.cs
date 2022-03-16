using UnityEngine;

namespace DungeonQuest.Pickups
{
	public class HealthPickup : MonoBehaviour
	{
		[SerializeField] private bool destroyOnPickup;

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.CompareTag("Player"))
			{
				collider.GetComponent<Player.PlayerHealing>().HealingPotions++;

				if (destroyOnPickup) Destroy(gameObject);
			}
		}
	}
}
