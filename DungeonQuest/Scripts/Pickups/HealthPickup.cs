using UnityEngine;

namespace DungeonQuest.Pickups
{
	public class HealthPickup : MonoBehaviour
	{
		[SerializeField] private bool destroyOnPickup;

		private Collider2D playerCollider;

		void Awake()
		{
			playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (playerCollider == collider)
			{
				collider.GetComponent<Player.PlayerHealing>().HealingPotions++;

				if (destroyOnPickup) Destroy(gameObject);
			}
		}
	}
}
