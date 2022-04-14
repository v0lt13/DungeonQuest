using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DungeonQuest.Player
{
	public class PlayerRespawning : MonoBehaviour
	{
		[SerializeField] private GameObject gameOverScreen;

		private float respawnCooldown = 3f;
		private bool canRespawn;

		private PlayerManager playerManager;

		void Awake() 
		{
			playerManager = GetComponent<PlayerManager>();
		}
		
		void Update()
		{
			canRespawn = respawnCooldown < 1f;
			
			if (playerManager.lifeCount == 0)
			{
				Invoke("EnableGameOverScreen", 3f);
			}

			if (playerManager.isDead && respawnCooldown > 0f && playerManager.lifeCount != 0)
			{
				respawnCooldown -= Time.deltaTime;
			}

			if (Input.GetButtonDown("Respawn") && playerManager.isDead && canRespawn)
			{
				playerManager.isDead = false;
				rigidbody2D.isKinematic = false;

				collider2D.enabled = true;
				playerManager.playerAttack.enabled = true;
				playerManager.playerHealing.enabled = true;
				playerManager.playerMovement.enabled = true;
				playerManager.enabled = true;

				canRespawn = false;
				respawnCooldown = 3f;

				playerManager.HealPlayer(int.MaxValue);
				StartCoroutine(RespawnInvicibility());
			}
		}

		private void EnableGameOverScreen()
		{
			GameManager.EnableCursor(true);
			GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);

			gameOverScreen.SetActive(true);
		}

		private IEnumerator RespawnInvicibility()
		{
			playerManager.invisible = true;
			playerManager.GodMode = true;

			yield return new WaitForSeconds(3f);

			playerManager.invisible = false;
			playerManager.GodMode = false;
		}
	}
}
