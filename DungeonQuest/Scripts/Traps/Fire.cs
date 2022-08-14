using UnityEngine;
using System.Collections;
using DungeonQuest.Player;

namespace DungeonQuest.Traps
{
	public class Fire : MonoBehaviour 
	{
		[SerializeField] private float damageInterval;

		private bool isPlayerOnFire;
		private bool hasCoroutineStarted;

		private PlayerManager playerManager;

		void Awake() 
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		}

		void Update()
		{
			if (!hasCoroutineStarted && isPlayerOnFire) StartCoroutine(DamagePlayer());
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (playerManager.collider2D == collider) isPlayerOnFire = true;
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (playerManager.collider2D == collider) isPlayerOnFire = false;
		}

		private IEnumerator DamagePlayer()
		{
			hasCoroutineStarted = true;

			while (isPlayerOnFire && !playerManager.isDead)
			{
				// Damage 25% of the players total health
				playerManager.DamagePlayer(playerManager.defaultPlayerHealth / 4);

				yield return new WaitForSeconds(damageInterval);
			}

			hasCoroutineStarted = false;
		}
	}
}
