using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Traps
{
	public class IceArea : MonoBehaviour
	{
		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (playerManager.playerCollider == collider)
			{
				playerManager.playerMovement.isWalkingOnIce = true;
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (playerManager.playerCollider == collider)
			{
				playerManager.playerMovement.isWalkingOnIce = false;
			}
		}
	}
}
