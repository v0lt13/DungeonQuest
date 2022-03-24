using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Pickups
{
	public class CoinsPickup : MonoBehaviour
	{
		[SerializeField] private int amountGiven;
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
				var player = collider.GetComponent<PlayerManager>();

				player.GiveCoins(amountGiven);
				GetComponent<AudioSource>().Play();

				if (destroyOnPickup)
				{
					GetComponent<SpriteRenderer>().enabled = false;
					GetComponent<BoxCollider2D>().enabled = false;
					Destroy(gameObject, 1f);
				}
			}
		}
	}
}
