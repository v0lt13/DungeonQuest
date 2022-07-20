using UnityEngine;
using DungeonQuest.Player;

namespace DungeonQuest.Traps
{
	public class GooArea : MonoBehaviour 
	{
		private PlayerManager playerManager;

		void Awake()
		{
			playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (playerManager.collider2D == collider)
			{
				playerManager.playerMovement.isWalkingOnGoo = true;
			}
		}

		void OnTriggerExit2D(Collider2D collider)
		{
			if (playerManager.collider2D == collider)
			{
				playerManager.playerMovement.isWalkingOnGoo = false;
			}
		}
	}
}
