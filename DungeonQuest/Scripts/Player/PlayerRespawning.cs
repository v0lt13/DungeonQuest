﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DungeonQuest.Player
{
	public class PlayerRespawning : MonoBehaviour
	{
		[SerializeField] private GameObject gameOverScreen;
		[SerializeField] private Text respawnText;

		private float respawnCooldown = 5f;
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
				Invoke("EnableGameOverScreen", 2f);
			}

			if (playerManager.isDead && respawnCooldown > 0f && playerManager.lifeCount != 0)
			{
				respawnCooldown -= Time.deltaTime;

				respawnText.gameObject.SetActive(true);
				respawnText.text = "Respawning in: " + respawnCooldown.ToString("n0");
			}

			if (canRespawn)
			{
				playerManager.isDead = false;
				rigidbody2D.isKinematic = false;

				collider2D.enabled = true;
				playerManager.playerAttack.enabled = true;
				playerManager.playerHealing.enabled = true;
				playerManager.playerMovement.enabled = true;
				playerManager.enabled = true;

				canRespawn = false;
				respawnCooldown = 5f;

				respawnText.gameObject.SetActive(false);
				playerManager.HealPlayer(int.MaxValue);
				StartCoroutine(RespawnInvicibility());
			}
		}

		private void EnableGameOverScreen()
		{
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
